using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardVisuals))]
public class CardInventory : MonoBehaviour
{
    public static bool IsDragging;
    private Vector3 offset;
    private Slot originalSlot;
    private float zCoord;
    private float fixedYPosition;
    private bool isOverSlot;
    private int originalLayer;
    private Card _card;

    private bool isZoomed;
    private Vector3 originalPosition;
    // private Quaternion originalRotation;
    private Vector3 originalScale;
    private int originalSortingOrder;
    private Coroutine zoomCoroutine;
    private bool inputProcessedThisFrame;

    private float zoomScale = 4f;
    private float zoomDuration = 0.3f;

    private GameObject backgroundDim;
    private Canvas uiCanvas;
    public Color dimColor = new Color(0, 0, 0, 0.7f);


    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click detection
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                if (!isZoomed) ToggleZoom();
                inputProcessedThisFrame = true;
            }
        }

        // Handle click anywhere to zoom out
        if (isZoomed && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if (!inputProcessedThisFrame) ToggleZoom();
        }

        // Reset input flag at frame end
        StartCoroutine(ResetInputFlag());
    }

    void Start()
    {
        _card = GetComponent<CardVisuals>().card;
    }


    void OnMouseDown()
    {
        if (isZoomed)
        {
            // ToggleZoom();
            return; // Prevent dragging while zoomed
        }

        IsDragging = true;

        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Drag");

        originalSlot = GetComponentInParent<Slot>();

        if (originalSlot != null)
        {
            originalSlot.isOccupied = false;
            originalSlot.currentCard = null;
        }

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        fixedYPosition = transform.position.y + 0.2f;
        transform.position = new Vector3(
            transform.position.x,
            fixedYPosition,
            transform.position.z
        );

        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (!IsDragging) return;

        Vector3 newPosition = GetMouseWorldPos() + offset;
        newPosition.y = fixedYPosition; // Maintain original Y position
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        if (!IsDragging) return;

        IsDragging = false;
        gameObject.layer = originalLayer;
        Slot newSlot = FindNearestSlot();

        if (newSlot != null && InventoryManager.Instance.IsValidTransfer(originalSlot, newSlot))
        {
            MoveToSlot(newSlot);
        }
        else
        {
            ReturnToOriginalSlot();
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePoint);
        worldPos.y = fixedYPosition; // Lock Y to initial value
        return worldPos;
    }

    Slot FindNearestSlot()
    {
        Slot[] slots = FindObjectsByType<Slot>(FindObjectsSortMode.None);
        Slot nearestSlot = null;
        float minDistance = Mathf.Infinity;

        foreach (Slot slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < minDistance && distance < 1f) // 1m radius
            {
                minDistance = distance;
                nearestSlot = slot;
            }
        }
        return nearestSlot;
    }

    void MoveToSlot(Slot newSlot)
    {
        if (newSlot.inventoryType != originalSlot.inventoryType)
        {
            switch (newSlot.inventoryType)
            {
                case InventoryType.Chest:
                    DeckManager.Instance.RemoveCardFromDeck(_card);
                    DeckManager.Instance.AddCardToChest(_card);
                    Debug.Log($"{_card.cardName} added to Chest");
                    break;
                case InventoryType.Deck:
                    DeckManager.Instance.RemoveCardFromChest(_card);
                    DeckManager.Instance.AddCardToDeck(_card);
                    Debug.Log($"{_card.cardName} added to Deck");
                    break;
                case InventoryType.Upgrade:
                    Debug.Log($"{_card.cardName} added to Upgrade");
                    break;
                default:
                    Debug.Log("Neither Inventory");
                    break;
            }
        }

        originalSlot.currentCard = null;
        newSlot.currentCard = gameObject;
        newSlot.isOccupied = true;
        StartCoroutine(MoveToPosition(newSlot.transform.position));
        transform.SetParent(newSlot.transform);
    }

    void ReturnToOriginalSlot()
    {
        if (originalSlot != null)
        {
            originalSlot.isOccupied = true;
            Vector3 targetPos = originalSlot.transform.position;
            StartCoroutine(MoveToPosition(targetPos));
        }
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        float duration = 0.2f;
        Vector3 startPos = transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }

    IEnumerator ResetInputFlag()
    {
        yield return new WaitForEndOfFrame();
        inputProcessedThisFrame = false;
    }

    void ToggleZoom()
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        if (isZoomed)
        {
            zoomCoroutine = StartCoroutine(ZoomOut());
        }
        else
        {
            StoreOriginalValues();
            zoomCoroutine = StartCoroutine(ZoomIn());
        }
        isZoomed = !isZoomed;
    }

    void StoreOriginalValues()
    {
        originalPosition = transform.position;
        // originalRotation = transform.rotation;
        originalScale = transform.localScale;

        if (TryGetComponent<Canvas>(out var canvas))
        {
            originalSortingOrder = canvas.sortingOrder;
        }
    }

    IEnumerator ZoomIn()
    {
        // Activate background dim
        // backgroundDim.alpha = 0.7f;
        // backgroundDim.blocksRaycasts = true;

        CreateBackgroundDim();

        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
        // Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward);

        float elapsed = 0;
        while (elapsed < zoomDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / zoomDuration);
            // transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsed/zoomDuration);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * zoomScale, elapsed / zoomDuration);

            if (TryGetComponent<Canvas>(out var canvas))
            {
                canvas.sortingOrder = 100;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ZoomOut()
    {
        float elapsed = 0;
        Vector3 startPosition = transform.position;
        // Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;

        while (elapsed < zoomDuration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed / zoomDuration);
            // transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsed/zoomDuration);
            transform.localScale = Vector3.Lerp(startScale, originalScale, elapsed / zoomDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset final values
        transform.position = originalPosition;
        // transform.rotation = originalRotation;
        transform.localScale = originalScale;

        if (TryGetComponent<Canvas>(out var canvas))
        {
            canvas.sortingOrder = originalSortingOrder;
        }

        // Destroy background
        if (backgroundDim != null)
        {
            Destroy(backgroundDim);
            backgroundDim = null;
        }

        // Deactivate background dim
        // backgroundDim.alpha = 0f;
        // backgroundDim.blocksRaycasts = false;
    }

    void CreateBackgroundDim()
    {
        // Find or create UI Canvas
        if (uiCanvas == null)
        {
            var canvasObj = GameObject.Find("DynamicUICanvas");
            if (canvasObj == null)
            {
                canvasObj = new GameObject("DynamicUICanvas");
                uiCanvas = canvasObj.AddComponent<Canvas>();
                uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<GraphicRaycaster>();
            }
            else
            {
                uiCanvas = canvasObj.GetComponent<Canvas>();
            }
        }

        // Create background panel
        backgroundDim = new GameObject("BackgroundDim");
        backgroundDim.transform.SetParent(uiCanvas.transform);

        // Add full-screen rect transform
        RectTransform rt = backgroundDim.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Add visual components
        Image bgImage = backgroundDim.AddComponent<Image>();
        bgImage.color = dimColor;

        // Add click handling
        BackgroundDimController dimController = backgroundDim.AddComponent<BackgroundDimController>();
        dimController.card = this;

        // Add raycast blocking
        CanvasGroup cg = backgroundDim.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true;
    }
}


public class BackgroundDimController : MonoBehaviour, IPointerClickHandler
{
    public Card card;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (card != null)
        {
            card.ToggleZoom();
        }
    }
}
