using System;
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
    public GameObject floatingTextPrefab;


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
        Debug.Log("Player's start turn mana increased to " + startTurnMana);
    }

    public void Heal(int amount)
    {
        health = Math.Min(maxHealth, health + amount);
        healthBar.SetHealth(health);
        if (floatingTextPrefab)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            textObj.GetComponent<FloatingText>().SetText(amount);
        }
        Debug.Log("Player healed by " + amount + ". Current health: " + health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (floatingTextPrefab)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            textObj.GetComponent<FloatingText>().SetText(amount);
        }
        Debug.Log("Player took " + amount + " damage. Current health: " + health);
    }

    public void GainMana(int amount)
    {
        mana += amount;
        Debug.Log("Player gained " + amount + " mana. Current mana: " + mana);
    }
}