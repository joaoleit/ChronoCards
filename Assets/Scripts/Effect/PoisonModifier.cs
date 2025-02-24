public class PoisonModifier : ICardEffect, IModifier, ITurnListener
{
    private int stacks;
    private int damagePerStack;
    private Enemy target;
    private PoisonModifier modifier;


    public PoisonModifier(EffectData data)
    {
        damagePerStack = data.value;
        stacks = data.duration;
    }

    public PoisonModifier(EffectData data, Enemy enemy)
    {
        damagePerStack = data.value;
        stacks = data.duration;
        target = enemy;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        modifier = new PoisonModifier(GetEffectData(), enemy);
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public void OnTurnStart()
    {
        int finalDamage = damagePerStack;

        foreach (var modifier in EffectManager.Instance.GetModifiers<IDamageModifier>())
        {
            finalDamage = modifier.ModifyDamage(finalDamage);
        }

        target.TakeDamage(damagePerStack);
        stacks--;
    }

    public bool ShouldTriggerOnEnemy() => true;
    public string GetDescription() => $"Apply {stacks} stacks. They deal {damagePerStack} damage each turn.";
    public void UpgradeEffect()
    {
        stacks += 1;
        damagePerStack += 1;
    }
    public bool IsExpired() => stacks <= 0;

    public EffectData GetEffectData() => new EffectData { value = damagePerStack, duration = stacks };
    public ICardEffect Clone() => new PoisonModifier(GetEffectData());
}
