using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    public List<Card> chest = new List<Card>();
    public List<Card> deck = new List<Card>();
    public int maxDeckSize = 20;

    public void AddCardToDeck(Card card)
    {
        if (deck.Count < maxDeckSize)
        {
            deck.Add(card);
        }
    }
}
