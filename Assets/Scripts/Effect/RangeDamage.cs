using UnityEngine;

public class RangeDamage : ICardEffect
{
    private int baseValue;

    public RangeDamage(EffectData data)
    {
        baseValue = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        int minDamage = baseValue - 1;
        int maxDamage = baseValue + 2;
        int finalDamage = Random.Range(minDamage, maxDamage + 1); // Inclusive

        // Apply damage modifiers before dealing damage
        foreach (var modifier in EffectManager.Instance.GetModifiers<IDamageModifier>())
        {
            finalDamage = modifier.ModifyDamage(finalDamage);
        }

        enemy.TakeDamage(finalDamage);
    }

    public bool ShouldTriggerOnEnemy() => true;

    public string GetDescription() => $"Deal {baseValue - 1}-{baseValue + 2} damage.";

    public void UpgradeEffect()
    {
        // Increase base value by 1 on upgrade
        baseValue += 1;
    }

    public EffectData GetEffectData() => new EffectData { value = baseValue };
    public ICardEffect Clone() => new RangeDamage(GetEffectData());
}