using UnityEngine;

public class Card2 : MonoBehaviour
{
    public static bool IsDragging;
    private Vector3 offset;
    private Slot originalSlot;
    private float zCoord;
    private float fixedYPosition;
    private bool isOverSlot;
    private int originalLayer;


    void OnMouseDown()
    {
        IsDragging = true;
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        fixedYPosition = transform.position.y; // Store initial Y
        offset = transform.position - GetMouseWorldPos();
        originalSlot = GetComponentInParent<Slot>();
        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Drag");
        if (originalSlot != null) originalSlot.isOccupied = false;
        fixedYPosition = transform.position.y + 0.2f; // Lift card slightly
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (IsDragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            newPosition.y = fixedYPosition; // Maintain original Y position
            transform.position = newPosition;
        }
    }

    void OnMouseUp()
    {
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
        Slot[] slots = FindObjectsOfType<Slot>();
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
            // targetPos.y = fixedYPosition; // Maintain lifted position during return
            StartCoroutine(MoveToPosition(targetPos));
        }
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
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
}