public class HealPerCardModifier : IModifier, ICardPlayedListener, ITurnListener
{
    private int healAmount;
    private int duration;

    public HealPerCardModifier(int healAmount, int duration)
    {
        this.healAmount = healAmount;
        this.duration = duration;
    }

    public void OnCardPlayed(Card card)
    {
        Player player = GameManager.Instance.player;
        player.Heal(healAmount);
    }

    public void OnTurnStart()
    {
        duration--;
        if (IsExpired())
            GameEvents.Instance.OnModifierExpired.Invoke(this);
    }

    public bool IsExpired() => duration <= 0;
}