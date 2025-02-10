public class DoubleDamageModifier : IModifier, ITurnListener, IDamageModifier
{
    private int duration;

    public DoubleDamageModifier(int duration)
    {
        this.duration = duration;
    }

    public int ModifyDamage(int damage) => damage * 2;

    public void OnTurnStart()
    {
        duration--;
    }

    public bool IsExpired() => duration <= 0;
}