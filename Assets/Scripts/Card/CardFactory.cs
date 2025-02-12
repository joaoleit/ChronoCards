using UnityEngine;
using System.Collections.Generic;

public static class CardFactory
{
  public static Card CreateCard(string cardName, int manaCost, List<CardEffect> effects, Color color)
  {
    Card card = ScriptableObject.CreateInstance<Card>();
    card.cardName = cardName;
    card.manaCost = manaCost;
    card.effects = effects;
    card.color = color;
    card.description = GenerateDescription(effects); // Generate dynamic description
    return card;
  }

  private static string GenerateDescription(List<CardEffect> effects)
  {
    string description = "";
    foreach (var effect in effects)
    {
      switch (effect.effectType)
      {
        case CardEffect.EffectType.Damage:
          description += $"Deal {effect.value} damage. ";
          break;
        case CardEffect.EffectType.Heal:
          description += $"Heal {effect.value} health. ";
          break;
        case CardEffect.EffectType.Mana:
          description += $"Gain {effect.value} mana. ";
          break;
        case CardEffect.EffectType.Passive:
          description += GeneratePassiveDescription(effect.passiveModifier);
          break;
      }
    }
    return description.Trim(); // Remove trailing space
  }

  private static string GeneratePassiveDescription(PassiveModifier modifier)
  {
    switch (modifier.modifierType)
    {
      case PassiveModifier.ModifierType.DoubleDamageNextTurn:
        return "Double damage next turn. ";
      case PassiveModifier.ModifierType.BonusDamageNextCard:
        return $"Next card deals +{modifier.value} damage. ";
      case PassiveModifier.ModifierType.HealPerCardThisTurn:
        return $"Heal {modifier.value} health per card this turn. ";
      case PassiveModifier.ModifierType.DamagePerCard:
        return $"Deal {modifier.value} damage per card this turn. ";
      default:
        return "Unknown effect. ";
    }
  }
}
