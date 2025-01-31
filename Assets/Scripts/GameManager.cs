using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public GameObject handObject;
    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    void Start()
    {
        // Initialize game state
    }

    public void PlayCard(Card card)
    {
        if (/*hand.Contains(card) && */player.mana >= card.manaCost)
        {
            player.mana -= card.manaCost;
            card.PlayCard(player, enemy);
            //hand.Remove(card);
            //discardPile.Add(card);
            Debug.Log("Played card: " + card.cardName);
        }
        else
        {
            Debug.Log("Cannot play this card!");
        }
    }
    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            hand.Add(drawnCard);
            deck.RemoveAt(0);
            Debug.Log("Drew card: " + drawnCard.cardName);
        }
        else
        {
            Debug.Log("No cards left in the deck!");
        }
    }
}