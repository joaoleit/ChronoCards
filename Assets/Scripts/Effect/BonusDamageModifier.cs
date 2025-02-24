using UnityEngine;

public class BonusDamageModifier : ICardEffect, IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private int duration;
    private BonusDamageModifier modifier;

    public BonusDamageModifier(EffectData data)
    {
        bonusDamage = data.value;
        duration = data.duration;
    }

    public int ModifyDamage(int damage)
    {
        duration -= 1;
        return damage + bonusDamage;
    }

    public void OnCardPlayed(Card card) { }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new BonusDamageModifier(GetEffectData());
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;
    public string GetDescription() => $"Next {duration} damage effect{(duration > 1 ? "s" : "")} deals +{bonusDamage} damage.";
    public void UpgradeEffect()
    {
        bonusDamage += 1;
        duration += 1;
    }
    public bool IsExpired() => duration <= 0;
    public EffectData GetEffectData() => new EffectData { value = bonusDamage, duration = duration };
    public ICardEffect Clone() => new BonusDamageModifier(GetEffectData());
}