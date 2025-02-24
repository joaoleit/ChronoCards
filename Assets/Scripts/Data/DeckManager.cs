using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    public List<Card> chest = new List<Card>();
    public List<Card> deck = new List<Card>();
    public const int MaxDeckSize = 25;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddCardToDeck(Card card)
    {
        if (deck.Count < MaxDeckSize && !deck.Contains(card))
        {
            deck.Add(card);
        }
    }

    public void RemoveCardFromDeck(Card card)
    {
        if (deck.Contains(card))
        {
            deck.Remove(card);
        }
    }

    public void AddCardToChest(Card card)
    {
        if (!chest.Contains(card))
            chest.Add(card);
    }

    public void RemoveCardFromChest(Card card)
    {
        if (chest.Contains(card))
        {
            chest.Remove(card);
        }
    }
}
