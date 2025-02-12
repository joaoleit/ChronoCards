public class BonusDamageModifier : IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private bool isConsumed;

    public BonusDamageModifier(int bonusDamage)
    {
        this.bonusDamage = bonusDamage;
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

    public bool IsExpired() => isConsumed;
}