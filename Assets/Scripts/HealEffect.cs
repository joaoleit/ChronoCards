using UnityEngine;

public class HealEffect : ICardEffect
{
    private int healAmount;

    public HealEffect(int healAmount)
    {
        this.healAmount = healAmount;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        player.Heal(healAmount);
    }

    public bool ShouldTriggerOnEnemy()
    {
        return false;
    }
}