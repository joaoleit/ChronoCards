public class DrawEffect : ICardEffect
{
    private int drawAmount;

    public DrawEffect(EffectData data)
    {
        drawAmount = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        BattleManager.Instance.DrawCards(drawAmount);
    }

    public bool ShouldTriggerOnEnemy()
    {
        return false;
    }

    public string GetDescription()
    {
        return $"Draw {drawAmount} card{(drawAmount > 1 ? "s" : "")}.";
    }

    public void UpgradeEffect() => drawAmount += 1;
    public EffectData GetEffectData() => new EffectData { value = drawAmount };
    public ICardEffect Clone() => new DrawEffect(GetEffectData());
}