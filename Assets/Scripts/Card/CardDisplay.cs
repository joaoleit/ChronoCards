using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public new TMP_Text name;
    public TMP_Text description;
    public TMP_Text manaCost;
    public float playableAreaThreshold = 0.5f; // Now based on viewport Y (0-1)
    private Vector3 offset;
    public bool isDragging = false;
    private Renderer cardRenderer;
    private bool shouldTriggerOnEnemy = false;
    private Plane dragPlane; // Plane for mouse position calculation
    private Vector3 targetPosition; // Smoothed target position
    public float dragSpeed = 10f; // Smoothing speed

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
        if (BattleManager.Instance.currentTurn != BattleManager.TurnState.PlayerTurn) return;
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
        // Create a plane perpendicular to camera forward at card's position
        dragPlane = new Plane(-Camera.main.transform.forward, transform.position);
        UpdateMousePosition();
        offset = transform.position - targetPosition;
        isDragging = true;
    }

    private void DragCard()
    {
        if (isDragging)
        {
            UpdateMousePosition();
            targetPosition = targetPosition + offset;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -.1f);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dragSpeed);
        }
    }

    private void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (dragPlane.Raycast(ray, out distance))
        {
            targetPosition = ray.GetPoint(distance);
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
            BattleManager.Instance.PlayCard(this);
            return;
        }

        ReturnToOriginalPosition();
    }

    private bool TryTriggerEffectOnEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
                continue;

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                BattleManager.Instance.PlayCard(this);
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

    private bool IsInPlayableArea()
    {
        // Use viewport position for camera-relative area check
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.y > playableAreaThreshold;
    }
}