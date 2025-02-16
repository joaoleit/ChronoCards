using UnityEngine;

public class HealPerCardModifier : ICardEffect, IModifier, ICardPlayedListener, ITurnListener
{
    private int healAmount;
    private int duration;

    public HealPerCardModifier(EffectData data)
    {
        healAmount = data.value;
        duration = data.duration;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        EffectManager.Instance.AddModifier(this);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Heal {healAmount} health per card played for {duration} turn(s).";

    public void UpgradeEffect()
    {
        healAmount += 1;
        duration += 1;
    }

    public void OnCardPlayed(Card card)
    {
        Player player = BattleManager.Instance.player;
        player.Heal(healAmount);
    }

    public void OnTurnStart()
    {
        duration--;
        if (IsExpired())
            GameEvents.Instance.OnModifierExpired.Invoke(this);
    }

    public bool IsExpired() => duration <= 0;
}