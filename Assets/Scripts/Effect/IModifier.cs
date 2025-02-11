public interface IModifier
{
    bool IsExpired();
}

public interface ITurnListener
{
    void OnTurnStart();
}

public interface ICardPlayedListener
{
    void OnCardPlayed(Card card);
}

public interface IDamageModifier
{
    int ModifyDamage(int damage);
}

public interface IHealModifier
{
    int ModifyHeal(int heal);
}