using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private PersistentDataManager dataManager;
    [SerializeField] private int maxDeckSize = 20;

    public bool AddCardToDeck(Card card)
    {
        if (dataManager.currentSave.deckCards.Count >= maxDeckSize)
        {
            Debug.LogWarning("Deck is full!");
            return false;
        }

        dataManager.currentSave.deckCards.Add(card.cardName);
        dataManager.SavePersistentData();
        return true;
    }

    public bool RemoveCardFromDeck(string cardName)
    {
        bool removed = dataManager.currentSave.deckCards.Remove(cardName);
        if (removed) dataManager.SavePersistentData();
        return removed;
    }

    public List<Card> GetCurrentDeck(CardDatabase database)
    {
        List<Card> deck = new List<Card>();
        foreach (string cardName in dataManager.currentSave.deckCards)
        {
            Card card = database.GetCardByName(cardName);
            if (card != null) deck.Add(card);
        }
        return deck;
    }
}