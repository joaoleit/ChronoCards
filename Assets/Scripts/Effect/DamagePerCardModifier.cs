public class DamagePerCardModifier : ICardEffect, IModifier, ICardPlayedListener, ITurnEndListener
{
    private int damagePerCard;
    private int duration;
    private DamagePerCardModifier modifier;

    public DamagePerCardModifier(EffectData data)
    {
        damagePerCard = data.value;
        duration = data.duration;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new DamagePerCardModifier(new EffectData { duration = duration, value = damagePerCard });
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;

    public string GetDescription() => $"Deal {damagePerCard} damage per card, lasts {duration} turn(s).";

    public void UpgradeEffect()
    {
        damagePerCard += 1;
        duration += 1;
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

    public void OnTurnEnd()
    {
        duration--;
    }

    public bool IsExpired() => duration <= 0;
}