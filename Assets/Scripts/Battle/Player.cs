using System;
using EasyTransition;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public int maxMana = 10;
    public int mana = 0;
    public int startTurnMana = 0;
    public int startHandSize = 5;
    public HealthBar healthBar;

    public TransitionSettings transition;

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        if (health == 0)
        {
            health = maxHealth;
        }
    }

    public void IncrementStartTurnMana()
    {
        startTurnMana = Math.Min(maxMana, startTurnMana + 1);
    }

    public void Heal(int amount)
    {
        health = Math.Min(maxHealth, health + amount);
        healthBar.SetHealth(health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            TransitionManager.Instance.Transition(5, transition, 0);
        };
    }

    public void GainMana(int amount)
    {
        mana += amount;
    }
}