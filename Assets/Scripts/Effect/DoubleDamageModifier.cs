public class DoubleDamageModifier : ICardEffect, IModifier, ITurnEndListener, IDamageModifier
{
    private int duration;
    private bool isNextTurn = false;
    private DoubleDamageModifier modifier;

    public DoubleDamageModifier(EffectData data)
    {
        duration = data.duration;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new DoubleDamageModifier(new EffectData { duration = duration });
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Deal double damage for {duration} turn{(duration > 1 ? "s" : "")} (starts next turn).";

    public void UpgradeEffect() => duration += 1;

    public int ModifyDamage(int damage) => isNextTurn ? damage * 2 : damage;

    public void OnTurnEnd()
    {
        if (isNextTurn)
        {
            duration--;
        }

        isNextTurn = true;
    }

    public bool IsExpired()
    {
        return duration <= 0;
    }
    public EffectData GetEffectData() => new EffectData { duration = duration };
}