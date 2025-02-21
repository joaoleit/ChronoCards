using UnityEngine;

public class CardHover : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isHovered = false;
    private float hoverHeight = 1f; // Adjust this value to set the hover height
    private float speed = 10f; // Adjust this value to set the speed of the hover effect

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        var cardDisplay = GetComponent<CardLogic>();
        if (cardDisplay._isDragging) return;

        if (isHovered)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition + new Vector3(0, hoverHeight, 0), Time.deltaTime * speed);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, Time.deltaTime * speed);
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
    }

    void OnMouseExit()
    {
        isHovered = false;
    }

    public void setOriginalPosition(Vector3 position)
    {
        originalPosition = position;
    }

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }
}