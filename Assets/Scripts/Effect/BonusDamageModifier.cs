public class BonusDamageModifier : ICardEffect, IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private bool isConsumed;

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

    public void OnCardPlayed(Card card)
    {
        if (isConsumed)
        {
            GameEvents.Instance.OnModifierExpired.Invoke(this);
        }
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        EffectManager.Instance.AddModifier(this);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Next card deals +{bonusDamage} damage.";
    public void UpgradeEffect() => bonusDamage += 1;
    public bool IsExpired() => isConsumed;
}