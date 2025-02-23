using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Card data
    public List<CardData> deck = new List<CardData>();
    public List<CardData> chest = new List<CardData>();

    // Player state
    public Vector3 playerPosition;
    public float playerHealth;

    // Game progress
    public List<string> defeatedEnemies = new List<string>();
    public float enemyDifficulty;

    public SaveData(List<Card> deckCards, List<Card> chestCards, Vector3 position, float health, List<string> enemies, float difficultyFactor)
    {
        foreach (var card in deckCards)
        {
            deck.Add(new CardData(card));
        }
        foreach (var card in chestCards)
        {
            chest.Add(new CardData(card));
        }

        playerPosition = position;
        playerHealth = health;
        defeatedEnemies = enemies;
        enemyDifficulty = difficultyFactor;
    }

    public List<Card> GetDeckCards()
    {
        List<Card> cards = new List<Card>();
        foreach (var cardData in deck)
        {
            cards.Add(CardDataToCard(cardData));
        }
        return cards;
    }

    public List<Card> GetChestCards()
    {
        List<Card> cards = new List<Card>();
        foreach (var cardData in chest)
        {
            cards.Add(CardDataToCard(cardData));
        }
        return cards;
    }

    private Card CardDataToCard(CardData cardData)
    {
        Card card = ScriptableObject.CreateInstance<Card>();
        card.cardName = cardData.cardName;
        card.description = cardData.description;
        card.manaCost = cardData.manaCost;
        card.color = cardData.color;

        foreach (var effectData in cardData.effects)
        {
            ICardEffect effect = CreateEffectFromData(effectData);
            if (effect != null)
            {
                card.effects.Add(effect);
            }
        }

        return card;
    }

    private static ICardEffect CreateEffectFromData(CardEffectData cardEffectData)
    {
        var effectData = new EffectData { duration = cardEffectData.duration, value = cardEffectData.value };
        switch (cardEffectData.effectType)
        {
            case "AreaDamageEffect":
                return new AreaDamageEffect(effectData);
            case "BloodRitualEffect":
                return new BloodRitualEffect(effectData);
            case "BonusDamageModifier":
                return new BonusDamageModifier(effectData);
            case "ChainLightningEffect":
                return new ChainLightningEffect(effectData);
            case "DamageEffect":
                return new DamageEffect(effectData);
            case "DamagePerCardModifier":
                return new DamagePerCardModifier(effectData);
            case "DoubleDamageModifier":
                return new DoubleDamageModifier(effectData);
            case "DrawEffect":
                return new DrawEffect(effectData);
            case "HealEffect":
                return new HealEffect(effectData);
            case "HealPerCardModifier":
                return new HealPerCardModifier(effectData);
            case "ManaEffect":
                return new ManaEffect(effectData);
            case "PoisonModifier":
                return new PoisonModifier(effectData);
            case "RangeDamage":
                return new RangeDamage(effectData);
            case "ShieldEffect":
                return new ShieldEffect(effectData);
            case "ThornsModifier":
                return new ThornsModifier(effectData);
            default:
                Debug.LogWarning("Unknown effect type: " + cardEffectData.effectType);
                return null;
        }
    }
}

[System.Serializable]
public class CardData
{
    public string cardName;
    public string description;
    public int manaCost;
    public List<CardEffectData> effects;
    public Color color;

    public CardData(Card card)
    {
        cardName = card.cardName;
        description = card.description;
        manaCost = card.manaCost;
        color = card.color;
        effects = new List<CardEffectData>();

        foreach (var effect in card.effects)
        {
            effects.Add(new CardEffectData(effect));
        }
    }
}

[System.Serializable]
public class CardEffectData
{
    public string effectType;
    public int value;
    public int duration;

    public CardEffectData(ICardEffect effect)
    {
        effectType = effect.GetType().Name;
        value = effect.GetEffectData().value;
        duration = effect.GetEffectData().duration;
    }
}
