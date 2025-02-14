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
        int finalDamage = damagePerCard;

        // Apply damage modifiers before dealing damage
        foreach (var modifier in EffectManager.Instance.GetModifiers<IDamageModifier>())
        {
            finalDamage = modifier.ModifyDamage(finalDamage);
        }

        Enemy enemy = BattleManager.Instance.enemy;
        enemy.TakeDamage(finalDamage);
        // enemy.TakeDamage(damagePerCard);
    }

    public void OnTurnStart()
    {
        duration--;
        if (IsExpired())
            GameEvents.Instance.OnModifierExpired.Invoke(this);

    }

    public bool IsExpired() => duration <= 0;
}