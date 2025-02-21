using UnityEngine;

[RequireComponent(typeof(CardVisuals))]
public class CardLogic : MonoBehaviour
{
    public Card _card { get; private set; }
    public float playableAreaThreshold = 0.5f;
    public float dragSpeed = 10f;

    private Vector3 _offset;
    public bool _isDragging { get; private set; }
    private bool _shouldTriggerOnEnemy;
    private Plane _dragPlane;
    private Vector3 _targetPosition;

    private void Start()
    {
        _card = GetComponent<CardVisuals>().card;

        // Determine if card needs enemy target
        foreach (var effect in _card.effects)
        {
            if (effect is DamageEffect)
            {
                _shouldTriggerOnEnemy = true;
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        if (BattleManager.Instance.currentTurn != BattleManager.TurnState.PlayerTurn) return;
        StartDragging();
    }

    private void OnMouseDrag() => DragCard();
    private void OnMouseUp() => HandleCardRelease();

    private void StartDragging()
    {
        _dragPlane = new Plane(-Camera.main.transform.forward, transform.position);
        UpdateMousePosition();
        _offset = transform.position - _targetPosition;
        _isDragging = true;
    }

    private void DragCard()
    {
        if (!_isDragging) return;

        UpdateMousePosition();
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.1f);
        transform.position = Vector3.Lerp(transform.position, _targetPosition + _offset, Time.deltaTime * dragSpeed);
    }

    private void UpdateMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_dragPlane.Raycast(ray, out var distance))
        {
            _targetPosition = ray.GetPoint(distance);
        }
    }

    private void HandleCardRelease()
    {
        _isDragging = false;

        if (_shouldTriggerOnEnemy && TryTriggerEffectOnEnemy()) return;
        if (IsInPlayableArea()) BattleManager.Instance.PlayCard(this);
        else ReturnToOriginalPosition();
    }

    private bool TryTriggerEffectOnEnemy()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        foreach (var hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.gameObject == gameObject) continue;
            if (!hit.collider.CompareTag("Enemy")) continue;

            BattleManager.Instance.PlayCard(this);
            return true;
        }
        return false;
    }

    private bool IsInPlayableArea()
    {
        var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.y > playableAreaThreshold;
    }

    private void ReturnToOriginalPosition()
    {
        var cardHover = GetComponent<CardHover>();
        transform.localPosition = cardHover != null ?
            cardHover.GetOriginalPosition() :
            Vector3.zero;
    }
}