using UnityEngine;
using System.Collections.Generic;

public static class CardFactory
{
  public static Card CreateCard(string cardName, int manaCost, List<ICardEffect> effects, Color color)
  {
    Card card = ScriptableObject.CreateInstance<Card>();
    card.cardName = cardName;
    card.manaCost = manaCost;
    card.effects = effects;
    card.color = color;
    card.description = GenerateDescription(effects); // Generate dynamic description
    return card;
  }

  public static string GenerateDescription(List<ICardEffect> effects)
  {
    string description = "";
    foreach (var effect in effects)
    {
      description += effect.GetDescription() + " ";
    }
    return description.Trim(); // Remove trailing space
  }
}
