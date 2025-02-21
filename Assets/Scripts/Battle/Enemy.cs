using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float criticalChance = 0.1f;
    public float difficultyFactor = 1.0f;
    public int baseMaxHealth = 100;
    public int baseDamage = 10;
    public int maxHealth;
    public int damage;
    public int health;
    public HealthBar healthBar;
    public GameObject deathParticles;
    public GameObject enemyObject;
    private Animator anim;

    void Start()
    {
        InitializeAttributes();
        anim = enemyObject.GetComponent<Animator>();
        Debug.Log(anim);
    }

    // Recalculates maxHealth and damage based on the difficulty factor.
    public void InitializeAttributes()
    {
        maxHealth = Mathf.RoundToInt(baseMaxHealth * difficultyFactor);
        damage = Mathf.RoundToInt(baseDamage * difficultyFactor);
        health = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        Debug.Log("Enemy initialized: MaxHealth=" + maxHealth + ", Damage=" + damage);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        anim.SetTrigger("Punched");
        Debug.Log("Enemy took " + amount + " damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }


    public virtual int GetDamage()
    {
        // Define the variation percentage (e.g., 20% variation)
        float variationPercentage = 0.2f;

        // Calculate the minimum and maximum damage based on the base damage
        int minDamage = Mathf.RoundToInt(damage * (1 - variationPercentage));
        // Random.Range for ints is inclusive on the lower bound and exclusive on the upper bound,
        // so add 1 to include the max value.
        int maxDamage = Mathf.RoundToInt(damage * (1 + variationPercentage)) + 1;

        // Generate a random base damage within the calculated range
        int randomBaseDamage = Random.Range(minDamage, maxDamage);

        // Check for a critical hit; if true, apply a multiplier (e.g., 1.5x)
        if (Random.value < criticalChance * difficultyFactor)
        {
            Debug.Log("Critical hit!");
            return Mathf.RoundToInt(randomBaseDamage * 1.5f);
        }

        return randomBaseDamage;
    }

    public void Attack(Player player)
    {
        int actualDamage = GetDamage();
        player.TakeDamage(actualDamage);
        anim.SetTrigger("Attack");
        Debug.Log("Enemy attacked player for " + actualDamage + " damage.");
    }

    protected virtual int CalculateMoves()
    {
        int moves = 1; // Every enemy gets at least one move.
        int extraMoveSlots = 2; // Allow up to 2 extra moves.

        float probabilityScale = Mathf.Clamp01((difficultyFactor - 0.5f) / 4.5f);

        for (int i = 0; i < extraMoveSlots; i++)
        {
            // Chance to add a move scales dynamically with difficultyFactor.
            if (Random.value < 0.1f + (0.3f * probabilityScale))
            {
                moves++;
            }
        }

        return moves;
    }

    // ExecuteTurn now may perform multiple moves.
    public virtual IEnumerator ExecuteTurn(Player player)
    {
        int moves = CalculateMoves();
        Debug.Log("Enemy will take " + moves + " move(s) this turn.");
        for (int i = 0; i < moves; i++)
        {
            yield return new WaitForSeconds(2f);
            Attack(player);
        }
        yield return new WaitForSeconds(2f);
    }

    // public void Attack(Player player)
    // {
    //     player.TakeDamage(damage);
    //     Debug.Log("Enemy attacked player for " + damage + " damage.");
    // }

    void Die()
    {
        GameEvents.Instance.OnEnemyDeath.Invoke();
        gameObject.SetActive(false);
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        ps.Stop();
        Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax);
        Destroy(gameObject, 2f);
    }
}