public class HealPerCardModifier : ICardEffect, IModifier, ICardPlayedListener, ITurnListener
{
    private int healAmount;
    private int duration;
    private HealPerCardModifier modifier;

    public HealPerCardModifier(EffectData data)
    {
        healAmount = data.value;
        duration = data.duration;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new HealPerCardModifier(new EffectData { duration = duration, value = healAmount });
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Heal {healAmount} per card played. Lasts {duration} turn(s).";

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
    }

    public bool IsExpired() => duration <= 0;
}