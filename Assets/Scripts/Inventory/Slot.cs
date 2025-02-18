using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isOccupied;
    public GameObject currentCard;
    public InventoryType inventoryType;
    private Material originalMaterial;
    private Color originalColor;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        originalColor = originalMaterial.color;
    }

    void Update()
    {
        if (Card2.IsDragging)
        {
            CheckMouseOverlap();
        }
    }

    void CheckMouseOverlap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast specifically against slot layer
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Slot")))
        {
            if (hit.collider.gameObject == gameObject)
            {
                OnMouseEnter();
            }
            else
            {
                OnMouseExit();
            }
        }
    }

    void OnMouseEnter()
    {
        if (Card2.IsDragging)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }
}