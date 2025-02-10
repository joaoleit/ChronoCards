public class HealPerCardModifier : IModifier, ICardPlayedListener
{
    private int healAmount;

    public HealPerCardModifier(int healAmount)
    {
        this.healAmount = healAmount;
    }

    public void OnCardPlayed(Card card)
    {
        Player player = GameManager.Instance.player;
        player.Heal(healAmount);
    }

    public bool IsExpired() => false; // Expires at end of turn (add ITurnListener if needed)
}