using UnityEngine;

[System.Serializable]
public class CardEffect
{
    public enum EffectType
    {
        Damage,
        Heal,
        Mana
    }

    public EffectType effectType;
    public int value;

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
            default:
                return null;
        }
    }
}