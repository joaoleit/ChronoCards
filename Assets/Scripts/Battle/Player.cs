using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHealth = 100;
    public int health;
    public int mana = 10;

    private void Start()
    {
        if (health == 0)
        {
            health = MaxHealth;
        }
    }

    public void Heal(int amount)
    {
        health = Math.Min(MaxHealth, health + amount);
        Debug.Log("Player healed by " + amount + ". Current health: " + health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player took " + amount + " damage. Current health: " + health);
    }

    public void GainMana(int amount)
    {
        mana += amount; 
        Debug.Log("Player gained " + amount + " mana. Current mana: " + mana);
    }
}