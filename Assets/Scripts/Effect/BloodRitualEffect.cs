public class BloodRitualEffect : ICardEffect
{
    private int healthCost;

    public BloodRitualEffect(EffectData data)
    {
        healthCost = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        if (GameManager.Instance.playerHealth > healthCost)
        {
            player.TakeDamage(healthCost);
            player.GainMana(healthCost * 2);
        }
    }

    public bool ShouldTriggerOnEnemy() => false;
    public string GetDescription() => $"Pay {healthCost} health for {healthCost * 2} mana";
    public void UpgradeEffect() => healthCost += 1;

    public EffectData GetEffectData() => new EffectData { value = healthCost };
}