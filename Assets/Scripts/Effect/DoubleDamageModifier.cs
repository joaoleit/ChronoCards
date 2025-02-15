using UnityEngine;

public class DoubleDamageModifier : ICardEffect, IModifier, ITurnEndListener, IDamageModifier
{
    private int duration;
    private bool isNextTurn = false;

    public DoubleDamageModifier(EffectData data)
    {
        duration = data.duration;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        EffectManager.Instance.AddModifier(this);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => "Deal double damage for " + duration + " turn(s).";

    public void UpgradeEffect() => duration += 1;

    public int ModifyDamage(int damage) => isNextTurn ? damage * 2 : damage;

    public void OnTurnEnd()
    {
        if (isNextTurn)
        {
            duration--;
            if (IsExpired())
                GameEvents.Instance.OnModifierExpired.Invoke(this);
        }

        isNextTurn = true;
    }

    public bool IsExpired()
    {
        return duration <= 0;
    }
}