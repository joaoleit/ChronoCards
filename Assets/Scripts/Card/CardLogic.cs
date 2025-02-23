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
            if (effect.ShouldTriggerOnEnemy())
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
        if (BattleManager.Instance.currentTurn != BattleManager.TurnState.PlayerTurn) return;
        _isDragging = false;

        if (_shouldTriggerOnEnemy)
        {
            if (TryTriggerEffectOnEnemy()) return;

            ReturnToOriginalPosition();
            return;
        }
        if (IsInPlayableArea() && BattleManager.Instance.PlayCard(this, null)) return;

        ReturnToOriginalPosition();
    }

    private bool TryTriggerEffectOnEnemy()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        foreach (var hit in Physics.RaycastAll(ray))
        {
            if (!hit.collider.CompareTag("Enemy")) continue;
            Enemy enemy = GetActiveScript(hit.collider.gameObject);
            if (enemy == null) continue;

            return BattleManager.Instance.PlayCard(this, enemy);
        }
        PopUpManager.Instance.InstantiatePopUp("Card not played on an enemy");
        return false;
    }

    private bool IsInPlayableArea()
    {
        var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (!(viewportPos.y > playableAreaThreshold)) PopUpManager.Instance.InstantiatePopUp("Card not played on top half area");
        return viewportPos.y > playableAreaThreshold;
    }

    private void ReturnToOriginalPosition()
    {
        var cardHover = GetComponent<CardHover>();
        transform.localPosition = cardHover != null ?
            cardHover.GetOriginalPosition() :
            Vector3.zero;
    }

    private Enemy GetActiveScript(GameObject enemy)
    {
        Enemy enemyScript;
        if (enemy.GetComponent<Enemy>().isActiveAndEnabled)
        {
            enemyScript = enemy.GetComponent<Enemy>();
        }
        else if (enemy.GetComponent<SelfHealingEnemy>().isActiveAndEnabled)
        {
            enemyScript = enemy.GetComponent<SelfHealingEnemy>();
        }
        else
        {
            enemyScript = enemy.GetComponent<AggressiveEnemy>();
        }

        return enemyScript;
    }
}