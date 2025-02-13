using UnityEngine;
using System.Collections;

public class SelfHealingEnemy : Enemy
{
    public int healAmount = 15;
    public float healChance = 0.5f; // 50% chance to heal instead of attacking when health is low.

    public override IEnumerator ExecuteTurn(Player player)
    {
        int moves = CalculateMoves();
        Debug.Log("SelfHealingEnemy will take " + moves + " move(s) this turn.");
        for (int i = 0; i < moves; i++)
        {
            yield return new WaitForSeconds(1f);
            // Heal if health is below 50% and chance criteria is met.
            if (health < maxHealth * 0.5f && Random.value < healChance)
            {
                HealSelf();
            }
            else
            {
                Attack(player);
            }
        }
        yield return new WaitForSeconds(1f);
    }

    void HealSelf()
    {
        health += healAmount;
        if (health > maxHealth)
            health = maxHealth;
        healthBar.SetHealth(health);
        Debug.Log("SelfHealingEnemy healed self for " + healAmount);
    }
}
