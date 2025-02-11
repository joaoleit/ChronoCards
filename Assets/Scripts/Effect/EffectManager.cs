using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[DefaultExecutionOrder(-50)]
public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private List<IModifier> modifiers = new List<IModifier>();


    private void OnEnable()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnTurnStart.AddListener(HandleTurnStart);
            GameEvents.Instance.OnCardPlayed.AddListener(HandleCardPlayed);
            GameEvents.Instance.OnModifierAdded.AddListener(HandleNewModifier);
            GameEvents.Instance.OnModifierExpired.AddListener(RemoveModifier);
        }
        else
        {
            Debug.LogError("GameEvents.Instance is null in OnEnable");
        }
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnTurnStart.RemoveListener(HandleTurnStart);
        GameEvents.Instance.OnCardPlayed.RemoveListener(HandleCardPlayed);
        GameEvents.Instance.OnModifierAdded.RemoveListener(HandleNewModifier);
        GameEvents.Instance.OnModifierExpired.RemoveListener(RemoveModifier);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void HandleTurnStart()
    {
        foreach (var modifier in modifiers.ToList())
        {
            if (modifier is ITurnListener turnListener)
                turnListener.OnTurnStart();

            if (modifier.IsExpired())
                GameEvents.Instance.OnModifierExpired.Invoke(modifier);
        }
    }

    private void HandleCardPlayed(Card card)
    {
        foreach (var modifier in modifiers.ToList())
        {
            if (modifier is ICardPlayedListener cardListener)
                cardListener.OnCardPlayed(card);

            if (modifier.IsExpired())
                GameEvents.Instance.OnModifierExpired.Invoke(modifier);
        }
    }

    private void HandleNewModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
        if (modifier is ITurnListener)
            GameEvents.Instance.OnTurnStart.AddListener(((ITurnListener)modifier).OnTurnStart);

        if (modifier is ICardPlayedListener)
            GameEvents.Instance.OnCardPlayed.AddListener(((ICardPlayedListener)modifier).OnCardPlayed);
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);

        // Clean up event connections
        if (modifier is ITurnListener)
            GameEvents.Instance.OnTurnStart.RemoveListener(((ITurnListener)modifier).OnTurnStart);

        if (modifier is ICardPlayedListener)
            GameEvents.Instance.OnCardPlayed.RemoveListener(((ICardPlayedListener)modifier).OnCardPlayed);
    }

    public IEnumerable<T> GetModifiers<T>() where T : class
    {
        return modifiers.OfType<T>();
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
    }
}