using UnityEngine;

public class ManaEffect : ICardEffect
{
    private int manaAmount;

    public ManaEffect(int manaAmount)
    {
        this.manaAmount = manaAmount;
    }

    public void ApplyEffect(Player player, Enemy enemy)
    {
        player.GainMana(manaAmount);
    }
}
