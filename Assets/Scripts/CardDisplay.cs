using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public new TMP_Text name;
    public TMP_Text description;
    public TMP_Text manaCost;
    public GameManager manager;
    public float playableAreaThreshold = 0.5f; // Configurable threshold for playable area (0.5 means middle of the screen)
    private Vector3 offset;
    private bool isDragging = false;
    private Renderer cardRenderer;
    private bool shouldTriggerOnEnemy = false;

    void Start()
    {
        name.text = card.cardName;
        description.text = card.description;
        manaCost.text = card.manaCost.ToString();
        cardRenderer = gameObject.GetComponent<Renderer>();
        cardRenderer.material.color = card.color;
        foreach (var effect in card.effects)
        {
            if (effect.effectType == CardEffect.EffectType.Damage)
            {
                shouldTriggerOnEnemy = true;
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseDrag()
    {
        DragCard();
    }

    private void OnMouseUp()
    {
        StopDragging();
        HandleCardDrop();
    }

    private void StartDragging()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    private void DragCard()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            newPosition.z = -1; // Set the z-axis position to -1
            transform.position = newPosition;
        }
    }

    private void StopDragging()
    {
        isDragging = false;
    }

    private void HandleCardDrop()
    {
        if (shouldTriggerOnEnemy)
        {
            if (TryTriggerEffectOnEnemy())
            {
                return;
            }
        }
        else if (IsInPlayableArea())
        {
            Debug.Log("Card dropped on playable area!");
            // manager.PlayCard(this);

            return;
        }

        ReturnToOriginalPosition();
    }

    private bool TryTriggerEffectOnEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && shouldTriggerOnEnemy)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Card dropped on enemy!");
                // manager.PlayCard(this);
                return true;
            }
        }

        return false;
    }

    private void ReturnToOriginalPosition()
    {
        CardHover cardHover = GetComponent<CardHover>();
        if (cardHover != null)
        {
            transform.localPosition = cardHover.GetOriginalPosition();
        }
        else
        {
            Debug.LogWarning("CardHover component not found!");
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
        return screenPoint.y > Screen.height * playableAreaThreshold;
    }
}