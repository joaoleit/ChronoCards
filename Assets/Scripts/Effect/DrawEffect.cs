using UnityEngine;

public class DrawEffect : ICardEffect
{
    private int drawAmount;

    public DrawEffect(int drawAmount)
    {
        this.drawAmount = drawAmount;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        GameManager.Instance.DrawCards(drawAmount);
    }

    public bool ShouldTriggerOnEnemy()
    {
        return false;
    }
}