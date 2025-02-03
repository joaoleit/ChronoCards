using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public new TMP_Text name;
    public TMP_Text description;
    public TMP_Text manaCost;
    public GameManager manager;
    private Vector3 offset;
    private bool isDragging = false;
    private int originalSortingOrder;
    private Renderer cardRenderer;

    void Start()
    {
        name.text = card.cardName;
        description.text = card.description;
        manaCost.text = card.manaCost.ToString();
        gameObject.GetComponent<Renderer>().material.color = card.color;
        cardRenderer = gameObject.GetComponent<Renderer>();
        originalSortingOrder = cardRenderer.sortingOrder;
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
        cardRenderer.sortingOrder = 10; // Set a high sorting order to appear on top
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            newPosition.z = -0.1f; // Set the z-axis position to -1
            transform.position = newPosition;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        cardRenderer.sortingOrder = originalSortingOrder; // Reset to original sorting order
        if (IsInPlayableArea())
        {
            manager.PlayCard(this);
        }
        else
        {
            // Return the card to its original position
            transform.localPosition = GetComponent<CardHover>().GetOriginalPosition();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private bool IsInPlayableArea()
    {
        // Implement your logic to check if the card is in the playable area
        // For example, you can use a collider or a specific area on the screen
        // Here is a simple example using screen coordinates
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        return screenPoint.y > Screen.height / 2; // Example condition: card is in the top half of the screen
    }
}