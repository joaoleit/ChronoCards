using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Card Game/Card Database")]
public class CardDatabase : ScriptableObject
{
  public List<Card> allCards = new List<Card>();

  public Card GetCardByName(string cardName)
  {
    return allCards.Find(c => c.cardName == cardName);
  }
}