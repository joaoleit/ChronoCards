public interface ICardEffect
{
    void ApplyEffect(Player player, Enemy enemy);
    bool ShouldTriggerOnEnemy();
    string GetDescription();
    void UpgradeEffect();
    EffectData GetEffectData();
}

public class EffectData
{
    public int value;
    public int duration;
}