using UnityEngine;
using System.Collections;

public class AggressiveEnemy : Enemy
{
    public int allyHealAmount = 10;
    public float allyHealChance = 0.3f; // 30% chance to heal an ally instead of attacking on a given move.

    // Optionally override CalculateMoves to further favor extra moves for aggressive enemies.
    protected override int CalculateMoves()
    {
        int moves = base.CalculateMoves();
        // For harder enemies, you might guarantee an extra move if the difficultyFactor is high.
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
            // Decide randomly whether to heal an ally or attack.
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
        // Implement your logic for selecting an ally and healing it.
        Debug.Log("AggressiveEnemy healed an ally for " + allyHealAmount);
    }
}
