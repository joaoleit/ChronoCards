using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public GameObject handObject;
    public GameObject cardPrefab;
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    public int MaxHandSize = 10;

    void Start()
    {
        // Initialize game state
        // LoadDeck();
    }

    public void PlayCard(CardDisplay cardDisplay)
    {
        Card card = cardDisplay.card;
        if (/*hand.Contains(card) && */player.mana >= card.manaCost)
        {
            player.mana -= card.manaCost;
            card.PlayCard(player, enemy);
            Debug.Log("Played card: " + card.cardName);
            Destroy(cardDisplay.gameObject);
            discardPile.Add(card);
            StartCoroutine(AlignCardsNextFrame());
        }
        else
        {
            Debug.Log("Cannot play this card!");
        }
    }

    public void DrawCard()
    {
        if (handObject.transform.childCount >= MaxHandSize)
        {
            Debug.Log("Hand is full!");
            return;
        }
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);

            // Instantiate the card display
            InstantiateCard(drawnCard);

            StartCoroutine(AlignCardsNextFrame());
            Debug.Log("Drew card: " + drawnCard.cardName);
        }
        else
        {
            Debug.Log("No cards left in the deck!");
        }
    }

    public void SaveDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            PlayerPrefs.SetString("DeckCard_" + i, JsonUtility.ToJson(deck[i]));
        }
        PlayerPrefs.SetInt("DeckCount", deck.Count);
        PlayerPrefs.Save();
        Debug.Log("Deck saved to PlayerPrefs.");
    }

    public void LoadDeck()
    {
        deck.Clear();
        int deckCount = PlayerPrefs.GetInt("DeckCount", 0);
        for (int i = 0; i < deckCount; i++)
        {
            string cardJson = PlayerPrefs.GetString("DeckCard_" + i);
            Card card = ScriptableObject.CreateInstance<Card>();
            JsonUtility.FromJsonOverwrite(cardJson, card);
            deck.Add(card);
        }
        Debug.Log("Deck loaded from PlayerPrefs.");
    }

    void OnApplicationQuit()
    {
        SaveDeck();
    }

    public void AlignCards()
    {
        int cardCount = handObject.transform.childCount;
        float cardWidth = 1.5f; // Adjust this value based on your card width
        float gap = 0.1f; // Adjust this value to set the gap between cards
        float totalWidth = cardCount * cardWidth + (cardCount - 1) * gap;
        float startX = -(totalWidth - cardWidth) / 2;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject cardObject = handObject.transform.GetChild(i).gameObject;
            Vector3 cardNewPosition = new Vector3(startX + i * (cardWidth + gap), 0, 0);
            cardObject.GetComponent<CardHover>().setOriginalPosition(cardNewPosition);
        }
    }

    private void InstantiateCard(Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab, handObject.transform);
        cardObject.GetComponent<CardDisplay>().card = card;
        cardObject.GetComponent<CardDisplay>().manager = this;
        cardObject.transform.localPosition = Vector3.right * 10;
    }

    private IEnumerator AlignCardsNextFrame()
    {
        yield return null; // Wait for the next frame
        AlignCards();
    }
}
