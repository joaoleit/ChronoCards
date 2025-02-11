public class BonusDamageModifier : IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private bool isConsumed;
    private bool isExpired;

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
        // Only expire after modifying damage once
        if (isConsumed)
        {
            isExpired = true;
        }
    }

    public bool IsExpired() => isExpired;
}