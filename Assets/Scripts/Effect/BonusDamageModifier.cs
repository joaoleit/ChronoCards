public class BonusDamageModifier : ICardEffect, IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private bool isConsumed;
    private int duration;
    private BonusDamageModifier modifier;

    public BonusDamageModifier(EffectData data)
    {
        bonusDamage = data.value;
        duration = data.duration;
    }

    public int ModifyDamage(int damage)
    {
        if (!isConsumed)
        {
            isConsumed = --duration == 0;
            return damage + bonusDamage;
        }
        return damage;
    }

    public void OnCardPlayed(Card card) { }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new BonusDamageModifier(new EffectData { value = bonusDamage });
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Next {duration} card{(duration > 1 ? "s" : "")} deals +{bonusDamage} damage.";
    public void UpgradeEffect() => bonusDamage += 1;
    public bool IsExpired() => isConsumed;
    public EffectData GetEffectData() => new EffectData { value = bonusDamage };
}