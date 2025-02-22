using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CardVisuals))]
public class CardInventory : MonoBehaviour
{
    public static bool IsDragging;
    private Vector3 offset;
    public Slot originalSlot { get; private set; }
    private float zCoord;
    private float fixedYPosition;
    private bool isOverSlot;
    private int originalLayer;
    private Card _card;

    private bool isZoomed;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private int originalSortingOrder;
    private Coroutine zoomCoroutine;
    private bool inputProcessedThisFrame;

    private float zoomScale = 4f;
    private float zoomDuration = 0.3f;

    void Update()
    {
        if (BlockingUI.IsBlocking) return;
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

        if (isZoomed && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if (!inputProcessedThisFrame) ToggleZoom();
        }

        StartCoroutine(ResetInputFlag());
    }

    void Start()
    {
        _card = GetComponent<CardVisuals>().card;
    }


    void OnMouseDown()
    {
        if (BlockingUI.IsBlocking || isZoomed) return;

        IsDragging = true;

        Slot currentSlot = GetComponentInParent<Slot>();
        if (
            currentSlot == null ||
            currentSlot.inventoryType != InventoryType.Combine ||
            currentSlot.inventoryType != InventoryType.Offer
        )
        {
            originalSlot = currentSlot;
        }

        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Drag");

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
        if (BlockingUI.IsBlocking || !IsDragging) return;

        Vector3 newPosition = GetMouseWorldPos() + offset;
        newPosition.y = fixedYPosition;
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        if (BlockingUI.IsBlocking || !IsDragging) return;

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
        worldPos.y = fixedYPosition;
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
                    Debug.Log($"{_card.cardName} added to Chest");
                    DeckManager.Instance.RemoveCardFromDeck(_card);
                    DeckManager.Instance.AddCardToChest(_card);
                    break;
                case InventoryType.Deck:
                    Debug.Log($"{_card.cardName} added to Deck");
                    DeckManager.Instance.RemoveCardFromChest(_card);
                    DeckManager.Instance.AddCardToDeck(_card);
                    break;
                case InventoryType.Offer:
                    Debug.Log($"{_card.cardName} added to offer");
                    break;
                case InventoryType.Combine:
                    Debug.Log($"{_card.cardName} added to Upgrade");
                    // DeckManager.Instance.RemoveCardFromDeck(_card);
                    // DeckManager.Instance.RemoveCardFromChest(_card);
                    // originalSlot.currentCard = null;
                    // Destroy(gameObject);
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

    public void ReturnToOriginalSlot()
    {
        if (originalSlot != null)
        {
            // Reset slot state
            originalSlot.isOccupied = true;
            originalSlot.currentCard = gameObject;

            // Ensure proper parent relationship
            transform.SetParent(originalSlot.transform);

            // Visual movement
            StartCoroutine(MoveToPosition(originalSlot.transform.position));
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
        originalScale = transform.localScale;

        if (TryGetComponent<Canvas>(out var canvas))
        {
            originalSortingOrder = canvas.sortingOrder;
        }
    }

    IEnumerator ZoomIn()
    {
        Vector3 targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));

        float elapsed = 0;
        while (elapsed < zoomDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / zoomDuration);
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
        Vector3 startScale = transform.localScale;

        while (elapsed < zoomDuration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed / zoomDuration);
            transform.localScale = Vector3.Lerp(startScale, originalScale, elapsed / zoomDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.localScale = originalScale;

        if (TryGetComponent<Canvas>(out var canvas))
        {
            canvas.sortingOrder = originalSortingOrder;
        }
    }
}

