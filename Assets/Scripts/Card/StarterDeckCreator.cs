using UnityEngine;
using System.Collections.Generic;

public class StarterDeckCreator : MonoBehaviour
{
    private readonly List<CardConfig> starterCards = new List<CardConfig>
    {
        // Damage Cards (5)
        new CardConfig("Strike", "Deal 5 damage", 1,
            new CardEffect { effectType = CardEffect.EffectType.Damage, value = 5 }, Color.red),
        new CardConfig("Fire Bolt", "Deal 7 damage", 2,
            new CardEffect { effectType = CardEffect.EffectType.Damage, value = 7 }, new Color(1, 0.5f, 0)),
        new CardConfig("Ice Shard", "Deal 4 damage", 1,
            new CardEffect { effectType = CardEffect.EffectType.Damage, value = 4 }, Color.cyan),
        new CardConfig("Thunder Clap", "Deal 6 damage", 2,
            new CardEffect { effectType = CardEffect.EffectType.Damage, value = 6 }, Color.yellow),
        new CardConfig("Poison Dart", "Deal 3 damage", 1,
            new CardEffect { effectType = CardEffect.EffectType.Damage, value = 3 }, Color.green),

        // Heal Cards (5)
        new CardConfig("Healing Touch", "Heal 5 health", 1,
            new CardEffect { effectType = CardEffect.EffectType.Heal, value = 5 }, Color.magenta),
        new CardConfig("Renew", "Heal 3 health", 1,
            new CardEffect { effectType = CardEffect.EffectType.Heal, value = 3 }, new Color(1, 0.8f, 0.9f)),
        new CardConfig("Greater Heal", "Heal 8 health", 3,
            new CardEffect { effectType = CardEffect.EffectType.Heal, value = 8 }, Color.white),
        new CardConfig("Bandage", "Heal 4 health", 1,
            new CardEffect { effectType = CardEffect.EffectType.Heal, value = 4 }, new Color(1, 0.2f, 0.2f)),
        new CardConfig("Vitality Boost", "Heal 6 health", 2,
            new CardEffect { effectType = CardEffect.EffectType.Heal, value = 6 }, new Color(0.5f, 1, 0.5f)),

        // Mana Cards (5)
        new CardConfig("Mana Crystal", "Gain 2 mana", 0,
            new CardEffect { effectType = CardEffect.EffectType.Mana, value = 2 }, Color.blue),
        new CardConfig("Arcane Intellect", "Gain 3 mana", 1,
            new CardEffect { effectType = CardEffect.EffectType.Mana, value = 3 }, new Color(0, 0.5f, 1)),
        new CardConfig("Energy Surge", "Gain 4 mana", 2,
            new CardEffect { effectType = CardEffect.EffectType.Mana, value = 4 }, Color.cyan),
        new CardConfig("Mystic Charge", "Gain 1 mana", 0,
            new CardEffect { effectType = CardEffect.EffectType.Mana, value = 1 }, new Color(0.7f, 0, 1)),
        new CardConfig("Power Flow", "Gain 2 mana", 1,
            new CardEffect { effectType = CardEffect.EffectType.Mana, value = 2 }, new Color(0, 1, 1)),

        // Passive Cards (5)
        new CardConfig("Preparation", "Next card deals +3 damage", 1,
            new CardEffect {
                effectType = CardEffect.EffectType.Passive,
                passiveModifier = new PassiveModifier {
                    modifierType = PassiveModifier.ModifierType.BonusDamageNextCard,
                    value = 3,
                    duration = 1
                }
            }, new Color(0.5f, 0.5f, 1)),
        new CardConfig("Fortify", "Heal 2 per card this turn", 2,
            new CardEffect {
                effectType = CardEffect.EffectType.Passive,
                passiveModifier = new PassiveModifier {
                    modifierType = PassiveModifier.ModifierType.HealPerCardThisTurn,
                    value = 2,
                    duration = 1
                }
            }, Color.gray),
        new CardConfig("Overload", "Deal 2 damage per card played", 3,
            new CardEffect {
                effectType = CardEffect.EffectType.Passive,
                passiveModifier = new PassiveModifier {
                    modifierType = PassiveModifier.ModifierType.DamagePerCard,
                    value = 2,
                    duration = 1
                }
            }, Color.black),
        new CardConfig("Double Strike", "Double damage next turn", 4,
            new CardEffect {
                effectType = CardEffect.EffectType.Passive,
                passiveModifier = new PassiveModifier {
                    modifierType = PassiveModifier.ModifierType.DoubleDamageNextTurn,
                    duration = 1
                }
            }, new Color(1, 0.8f, 0)),
        new CardConfig("Empower", "Next card gains +5 damage", 2,
            new CardEffect {
                effectType = CardEffect.EffectType.Passive,
                passiveModifier = new PassiveModifier {
                    modifierType = PassiveModifier.ModifierType.BonusDamageNextCard,
                    value = 5,
                    duration = 1
                }
            }, new Color(1, 0, 1))
    };

    private struct CardConfig
    {
        public string name;
        public string description;
        public int manaCost;
        public CardEffect effect;
        public Color color;

        public CardConfig(string name, string description, int manaCost, CardEffect effect, Color color)
        {
            this.name = name;
            this.description = description;
            this.manaCost = manaCost;
            this.effect = effect;
            this.color = color;
        }
    }

    public void CreateStarterDeck()
    {
        // Only create starter deck if no saved data exists
        foreach (var config in starterCards)
        {
            // Create card instance
            Card card = CardFactory.CreateCard(
                config.name,
                config.manaCost,
                new List<CardEffect> { config.effect },
                config.color
            );

            // Add to chest
            DeckManager.Instance.AddCardToChest(card);

            // Add to deck
            DeckManager.Instance.AddCardToDeck(card);
        }

        Debug.Log("Starter deck initialized with 20 cards");
    }
}