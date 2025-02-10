using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public int damage = 10;
    public HealthBar healthBar;
    public GameObject deathParticles; // Reference to the particle effect prefab

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        Debug.Log("Enemy took " + amount + " damage. Current health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Attack(Player player)
    {
        player.TakeDamage(damage);
        Debug.Log("Enemy attacked player for " + damage + " damage.");
    }

    void Die()
    {
        gameObject.tag = "Untagged"; // Remove the "Enemy" tag
        transform.Find("Gob1").gameObject.SetActive(false);
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity); // Instantiate particle effect
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        ps.Stop(); // Stop the particle system
        Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax); // Destroy after particle system duration
        Destroy(gameObject, 2f); // Destroy the enemy after 2 seconds
    }
}