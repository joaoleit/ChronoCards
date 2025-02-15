using UnityEngine;

public class HealEffect : ICardEffect
{
    private int healAmount;

    public HealEffect(EffectData data)
    {
        healAmount = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        player.Heal(healAmount);
    }

    public bool ShouldTriggerOnEnemy()
    {
        return false;
    }

    public string GetDescription()
    {
        return "Heal " + healAmount + " health.";
    }

    public void UpgradeEffect() => healAmount += 1;
}