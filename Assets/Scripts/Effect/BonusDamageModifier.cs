public class BonusDamageModifier : ICardEffect, IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private bool isConsumed;
    private BonusDamageModifier modifier;

    public BonusDamageModifier(EffectData data)
    {
        bonusDamage = data.value;
    }

    public int ModifyDamage(int damage)
    {
        if (!isConsumed)
        {
            isConsumed = true;
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

    public string GetDescription() => $"Next card deals +{bonusDamage} damage.";
    public void UpgradeEffect() => bonusDamage += 1;
    public bool IsExpired() => isConsumed;
    public EffectData GetEffectData() => new EffectData { value = bonusDamage };
}