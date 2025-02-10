// EffectManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private List<IModifier> modifiers = new List<IModifier>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
    }

    public IEnumerable<T> GetModifiers<T>() where T : class
    {
        return modifiers.OfType<T>();
    }

    // Triggered by GameManager when a new turn starts
    public void OnTurnStart()
    {
        foreach (var modifier in modifiers.ToList())
        {
            if (modifier is ITurnListener turnListener)
                turnListener.OnTurnStart();

            if (modifier.IsExpired())
                RemoveModifier(modifier);
        }
    }

    // Triggered when a card is played
    public void OnCardPlayed(Card card)
    {
        foreach (var modifier in modifiers.ToList())
        {
            if (modifier is ICardPlayedListener cardListener)
                cardListener.OnCardPlayed(card);

            if (modifier.IsExpired())
                RemoveModifier(modifier);
        }
    }
}