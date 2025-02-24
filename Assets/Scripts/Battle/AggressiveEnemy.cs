using UnityEngine;
using System.Collections;

public class AggressiveEnemy : Enemy
{
    public int allyHealAmount = 10;
    public float allyHealChance = 0.25f;

    public override void InitializeAttributes()
    {
        base.InitializeAttributes();
        allyHealAmount = Mathf.RoundToInt(maxHealth * 0.1f);
    }

    protected override int CalculateMoves()
    {
        int moves = base.CalculateMoves();
        if (difficultyFactor >= 2.0f)
        {
            moves++;
        }
        return moves;
    }

    public override IEnumerator ExecuteTurn(Player player)
    {
        int moves = CalculateMoves();
        Debug.Log("AggressiveEnemy will take " + moves + " move(s) this turn.");
        for (int i = 0; i < moves; i++)
        {
            yield return new WaitForSeconds(1f);
            if (Random.value < allyHealChance)
            {
                HealAlly();
            }
            else
            {
                Attack(player);
            }
        }
        yield return new WaitForSeconds(1f);
    }

    void HealAlly()
    {
        var enemies = BattleManager.Instance.enemies;
        foreach (var enemy in enemies)
        {
            if (enemy.health > 0)
            {
                enemy.health += allyHealAmount;
                if (enemy.health > enemy.maxHealth)
                    enemy.health = enemy.maxHealth;
                enemy.healthBar.SetHealth(enemy.health);
            }
        }
        // Implement your logic for selecting an ally and healing it.
        Debug.Log("AggressiveEnemy healed an ally for " + allyHealAmount);
    }
}
