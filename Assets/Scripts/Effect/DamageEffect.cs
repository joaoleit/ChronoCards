public class DamageEffect : ICardEffect
{
    private int damage;

    public DamageEffect(EffectData data)
    {
        damage = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        int finalDamage = damage;

        // Apply damage modifiers before dealing damage
        foreach (var modifier in EffectManager.Instance.GetModifiers<IDamageModifier>())
        {
            finalDamage = modifier.ModifyDamage(finalDamage);
        }

        enemy.TakeDamage(finalDamage);
    }

    public bool ShouldTriggerOnEnemy() => true;

    public string GetDescription() => "Deal " + damage + " damage.";
    public void UpgradeEffect() => damage += 1;
    public EffectData GetEffectData() => new EffectData { value = damage };
}