using UnityEngine;

public class DoubleDamageModifier : IModifier, ITurnEndListener, IDamageModifier
{
    private int duration;
    private bool isNextTurn = false;

    public DoubleDamageModifier(int duration)
    {
        this.duration = duration;
    }

    public int ModifyDamage(int damage) => isNextTurn ? damage * 2 : damage;

    public void OnTurnEnd()
    {
        if (isNextTurn)
        {
            duration--;
            if (IsExpired())
                GameEvents.Instance.OnModifierExpired.Invoke(this);
        }

        isNextTurn = true;
    }

    public bool IsExpired()
    {
        return duration <= 0;
    }
}