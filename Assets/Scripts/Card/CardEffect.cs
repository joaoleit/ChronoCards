using UnityEngine;

[System.Serializable]
public class CardEffect
{
    public enum EffectType
    {
        Damage,
        Heal,
        Mana,
        Passive
    }

    public EffectType effectType;
    public int value;

    // Used only if effectType == Passive
    public PassiveModifier passiveModifier;

    public ICardEffect GetEffect()
    {
        switch (effectType)
        {
            case EffectType.Damage:
                return new DamageEffect(value);
            case EffectType.Heal:
                return new HealEffect(value);
            case EffectType.Mana:
                return new ManaEffect(value);
            case EffectType.Passive:
                return new PassiveEffect(passiveModifier);
            default:
                return null;
        }
    }
}

[System.Serializable]
public class PassiveModifier
{
    public enum ModifierType
    {
        DoubleDamageNextTurn,
        BonusDamageNextCard,
        HealPerCardThisTurn,
        DamagePerCard
    }

    public ModifierType modifierType;
    public int value; // e.g., bonus damage amount
    public int duration; // in turns
}