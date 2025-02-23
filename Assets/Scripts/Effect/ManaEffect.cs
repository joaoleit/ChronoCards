public class ManaEffect : ICardEffect, IPlayAudioEffect
{
    private int manaAmount;

    public ManaEffect(EffectData data)
    {
        manaAmount = data.value;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        player.GainMana(manaAmount);
    }

    public bool ShouldTriggerOnEnemy()
    {
        return false;
    }

    public string GetDescription()
    {
        return "Gain " + manaAmount + " mana.";
    }

    public void UpgradeEffect() => manaAmount += 1;

    public AudioName GetAudioName() => AudioName.Mana;

    public EffectData GetEffectData() => new EffectData { value = manaAmount };
}
