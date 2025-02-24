using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CardFactory
{
  public static Card CreateCard(string cardName, int manaCost, List<ICardEffect> effects, Color color)
  {
    Card card = ScriptableObject.CreateInstance<Card>();
    card.cardName = cardName;
    card.manaCost = manaCost;
    card.effects = effects.Select(effect => effect.Clone()).ToList();
    card.color = color;
    card.description = GenerateDescription(effects); // Generate dynamic description
    return card;
  }

  public static Card CreateCard(Card _card)
  {
    Card card = ScriptableObject.CreateInstance<Card>();
    card.cardName = _card.cardName;
    card.manaCost = _card.manaCost;
    card.effects = _card.effects.Select(effect => effect.Clone()).ToList();
    card.color = _card.color;
    card.description = _card.description;
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
