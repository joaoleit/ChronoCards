using UnityEngine;
using System.Collections.Generic;

public class StarterDeckCreator : MonoBehaviour
{
    private static readonly List<CardConfig> starterCards = new List<CardConfig>
    {
        // Damage Cards (10)
        new CardConfig("Strike", "Deal 5 damage", 1,
            new DamageEffect(new EffectData { value = 5 }), Color.red),
        new CardConfig("Fire Bolt", "Deal 7 damage", 2,
            new DamageEffect(new EffectData { value = 7 }), new Color(1, 0.5f, 0)),
        new CardConfig("Ice Shard", "Deal 4 damage", 1,
            new DamageEffect(new EffectData { value = 4 }), Color.cyan),
        new CardConfig("Thunder Clap", "Deal 6 damage", 2,
            new DamageEffect(new EffectData { value = 6 }), Color.yellow),
        new CardConfig("Poison Dart", "Deal 3 damage", 1,
            new DamageEffect(new EffectData { value = 3 }), Color.green),
        new CardConfig("Shadow Strike", "Deal 8 damage", 3,
            new DamageEffect(new EffectData { value = 8 }), Color.black),
        new CardConfig("Magma Blast", "Deal 6 damage", 2,
            new DamageEffect(new EffectData { value = 6 }), Color.red),
        new CardConfig("Frost Nova", "Deal 4 damage", 1,
            new DamageEffect(new EffectData { value = 4 }), Color.blue),
        new CardConfig("Lightning Bolt", "Deal 7 damage", 3,
            new DamageEffect(new EffectData { value = 7 }), Color.yellow),
        new CardConfig("Venom Strike", "Deal 5 damage", 2,
            new DamageEffect(new EffectData { value = 5 }), Color.green),

        // Heal Cards (5)
        new CardConfig("Healing Touch", "Heal 5 health", 1,
            new HealEffect(new EffectData { value = 5 }), Color.magenta),
        new CardConfig("Renew", "Heal 3 health", 1,
            new HealEffect(new EffectData { value = 3 }), new Color(1, 0.8f, 0.9f)),
        new CardConfig("Greater Heal", "Heal 8 health", 3,
            new HealEffect(new EffectData { value = 8 }), Color.white),
        new CardConfig("Bandage", "Heal 4 health", 1,
            new HealEffect(new EffectData { value = 4 }), new Color(1, 0.2f, 0.2f)),
        new CardConfig("Vitality Boost", "Heal 6 health", 2,
            new HealEffect(new EffectData { value = 6 }), new Color(0.5f, 1, 0.5f)),

        // Mana Cards (5)
        new CardConfig("Mana Crystal", "Gain 2 mana", 0,
            new ManaEffect(new EffectData { value = 2 }), Color.blue),
        new CardConfig("Arcane Intellect", "Gain 3 mana", 1,
            new ManaEffect(new EffectData { value = 3 }), new Color(0, 0.5f, 1)),
        new CardConfig("Energy Surge", "Gain 4 mana", 2,
            new ManaEffect(new EffectData { value = 4 }), Color.cyan),
        new CardConfig("Mystic Charge", "Gain 1 mana", 0,
            new ManaEffect(new EffectData { value = 1 }), new Color(0.7f, 0, 1)),
        new CardConfig("Power Flow", "Gain 2 mana", 1,
            new ManaEffect(new EffectData { value = 2 }), new Color(0, 1, 1)),
        
        // Draw Cards (5)
        new CardConfig("Quick Draw", "Draw 1 card", 1,
            new DrawEffect(new EffectData { value = 1 }), Color.yellow),
        new CardConfig("Double Take", "Draw 2 cards", 2,
            new DrawEffect(new EffectData { value = 2 }), Color.green),
        new CardConfig("Triple Threat", "Draw 3 cards", 3,
            new DrawEffect(new EffectData { value = 3 }), Color.blue),
        new CardConfig("Quadruple Fortune", "Draw 4 cards", 4,
            new DrawEffect(new EffectData { value = 4 }), Color.magenta),
        new CardConfig("Pentadraw", "Draw 5 cards", 5,
            new DrawEffect(new EffectData { value = 5 }), Color.red),

        // Passive Cards (5)
        new CardConfig("Preparation", "Next card deals +3 damage", 1,
            new BonusDamageModifier(new EffectData { value = 3, duration = 1 }), new Color(0.5f, 0.5f, 1)),
        new CardConfig("Fortify", "Heal 2 per card this turn", 2,
            new HealPerCardModifier(new EffectData { value = 2, duration = 1 }), Color.gray),
        new CardConfig("Overload", "Deal 2 damage per card played", 3,
            new DamagePerCardModifier(new EffectData { value = 2, duration = 1 }), Color.black),
        new CardConfig("Double Strike", "Double damage next turn", 4,
            new DoubleDamageModifier(new EffectData { duration = 1 }), new Color(1, 0.8f, 0)),
        new CardConfig("Empower", "Next card gains +5 damage", 2,
            new BonusDamageModifier(new EffectData { value = 5, duration = 1 }), new Color(1, 0, 1))
    };

    private struct CardConfig
    {
        public string name;
        public string description;
        public int manaCost;
        public ICardEffect effect;
        public Color color;

        public CardConfig(string name, string description, int manaCost, ICardEffect effect, Color color)
        {
            this.name = name;
            this.description = description;
            this.manaCost = manaCost;
            this.effect = effect;
            this.color = color;
        }
    }

    public static void CreateStarterDeck()
    {
        // Only create starter deck if no saved data exists
        foreach (var config in starterCards)
        {
            // Create card instance
            Card card = CardFactory.CreateCard(
                config.name,
                config.manaCost,
                new List<ICardEffect> { config.effect },
                config.color
            );

            // Add to deck
            DeckManager.Instance.AddCardToDeck(card);
        }
    }

    public static List<Card> getRandomCards(int number)
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < number; i++)
        {
            int index = Random.Range(0, starterCards.Count);
            var config = starterCards[index];

            Card card = CardFactory.CreateCard(
                config.name,
                config.manaCost,
                new List<ICardEffect> { config.effect },
                config.color
            );

            cards.Add(card);
        }

        return cards;
    }
}