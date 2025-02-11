public class PassiveEffect : ICardEffect
{
    private PassiveModifier modifierData;

    public PassiveEffect(PassiveModifier modifierData)
    {
        this.modifierData = modifierData;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        IModifier modifier = null;

        switch (modifierData.modifierType)
        {
            case PassiveModifier.ModifierType.DoubleDamageNextTurn:
                modifier = new DoubleDamageModifier(modifierData.duration);
                break;
            case PassiveModifier.ModifierType.BonusDamageNextCard:
                modifier = new BonusDamageModifier(modifierData.value);
                break;
            case PassiveModifier.ModifierType.HealPerCardThisTurn:
                modifier = new HealPerCardModifier(modifierData.value, modifierData.duration);
                break;
            case PassiveModifier.ModifierType.DamagePerCard:
                modifier = new DamagePerCardModifier(modifierData.value, modifierData.duration);
                break;
        }

        if (modifier != null)
            EffectManager.Instance.AddModifier(modifier);
    }

    public bool ShouldTriggerOnEnemy() => false;
}