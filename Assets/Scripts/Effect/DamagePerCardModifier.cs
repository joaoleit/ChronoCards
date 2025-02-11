public class DamagePerCardModifier : IModifier, ICardPlayedListener, ITurnListener
{
    private int damagePerCard;
    private int duration;

    public DamagePerCardModifier(int damagePerCard, int duration)
    {
        this.damagePerCard = damagePerCard;
        this.duration = duration;
    }

    public void OnCardPlayed(Card card)
    {
        Enemy enemy = GameManager.Instance.enemy;
        enemy.TakeDamage(damagePerCard);
    }

    public void OnTurnStart()
    {
        duration--;
        if (IsExpired())
            GameEvents.Instance.OnModifierExpired.Invoke(this);

    }

    public bool IsExpired() => duration <= 0;
}