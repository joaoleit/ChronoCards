public class HealEffect : ICardEffect, IPlayAudioEffect
{
    private int healAmount;

    public HealEffect(EffectData data)
    {
        healAmount = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        player.Heal(healAmount);
    }

    public bool ShouldTriggerOnEnemy()
    {
        return false;
    }

    public string GetDescription()
    {
        return "Heal " + healAmount + ".";
    }

    public void UpgradeEffect() => healAmount += 1;

    public AudioName GetAudioName() => AudioName.Heal;

    public EffectData GetEffectData() => new EffectData { value = healAmount };
}