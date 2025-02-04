using UnityEngine;

public interface ICardEffect
{
    void ApplyEffect(Player player, Enemy enemy);
    bool ShouldTriggerOnEnemy();
}
