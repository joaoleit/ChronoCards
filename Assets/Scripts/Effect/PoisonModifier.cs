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
        modifier = new PoisonModifier(new EffectData { value = damagePerStack, duration = stacks }, enemy);
        GameEvents.Instance.OnModifierAdded.Invoke(modifier);
    }

    public void OnTurnStart()
    {
        target.TakeDamage(damagePerStack);
        stacks--;
    }

    public bool ShouldTriggerOnEnemy() => true;
    public string GetDescription() => $"Apply {stacks} stacks. They deal {damagePerStack} damage per turn.";
    public void UpgradeEffect()
    {
        stacks += 1;
        damagePerStack += 1;
    }
    public bool IsExpired() => stacks <= 0;

    public EffectData GetEffectData() => new EffectData { value = stacks, duration = damagePerStack };
}
