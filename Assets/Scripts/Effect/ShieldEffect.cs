using UnityEngine;

public class ShieldEffect : ICardEffect, IModifier, IIncomingDamageModifier
{
    private int shieldAmount;
    private ShieldEffect modifier;

    public ShieldEffect(EffectData data)
    {
        shieldAmount = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new ShieldEffect(new EffectData { value = shieldAmount });
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Gain {shieldAmount} shield.";

    public void UpgradeEffect()
    {
        shieldAmount += 2;
    }

    public int ModifyIncomingDamage(int damage)
    {
        Debug.Log(damage);
        int absorbed = Mathf.Min(shieldAmount, damage);
        shieldAmount -= absorbed;
        return damage - absorbed;
    }

    public bool IsExpired() => shieldAmount <= 0;

    public EffectData GetEffectData() => new EffectData { value = shieldAmount };
}