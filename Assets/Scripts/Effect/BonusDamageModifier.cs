public class BonusDamageModifier : IModifier, ICardPlayedListener, IDamageModifier
{
    private int bonusDamage;
    private bool isExpired;

    public BonusDamageModifier(int bonusDamage)
    {
        this.bonusDamage = bonusDamage;
    }

    public int ModifyDamage(int damage) => damage + bonusDamage;

    public void OnCardPlayed(Card card)
    {
        // Apply only once
        isExpired = true;
    }

    public bool IsExpired() => isExpired;
}