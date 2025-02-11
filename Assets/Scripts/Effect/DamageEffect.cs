using UnityEngine;

public class DamageEffect : ICardEffect
{
    private int damage;

    public DamageEffect(int damage)
    {
        this.damage = damage;
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
}