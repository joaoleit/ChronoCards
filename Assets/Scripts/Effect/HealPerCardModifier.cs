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
    }

    public bool IsExpired() => duration <= 0;
}