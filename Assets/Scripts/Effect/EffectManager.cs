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
        GameEvents.Instance.OnTurnStart.AddListener(CheckIsExpired);
        GameEvents.Instance.OnTurnEnd.AddListener(CheckIsExpired);
        GameEvents.Instance.OnCardPlayed.AddListener((Card card) => CheckIsExpired());
        GameEvents.Instance.OnModifierAdded.AddListener(HandleNewModifier);
        GameEvents.Instance.OnModifierExpired.AddListener(RemoveModifier);
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnTurnStart.RemoveListener(CheckIsExpired);
        GameEvents.Instance.OnTurnEnd.RemoveListener(CheckIsExpired);
        GameEvents.Instance.OnCardPlayed.RemoveListener((Card card) => CheckIsExpired());
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

    private void CheckIsExpired()
    {
        foreach (var modifier in modifiers.ToList())
        {
            // if (modifier is ITurnListener turnListener)
            //     turnListener.OnTurnStart();

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

        if (modifier is ITurnEndListener)
            GameEvents.Instance.OnTurnEnd.AddListener(((ITurnEndListener)modifier).OnTurnEnd);
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);

        // Clean up event connections
        if (modifier is ITurnListener)
            GameEvents.Instance.OnTurnStart.RemoveListener(((ITurnListener)modifier).OnTurnStart);

        if (modifier is ICardPlayedListener)
            GameEvents.Instance.OnCardPlayed.RemoveListener(((ICardPlayedListener)modifier).OnCardPlayed);

        if (modifier is ITurnEndListener)
            GameEvents.Instance.OnTurnEnd.AddListener(((ITurnEndListener)modifier).OnTurnEnd);
    }

    public IEnumerable<T> GetModifiers<T>() where T : class
    {
        return modifiers.OfType<T>();
    }
}