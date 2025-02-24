using System;
using EasyTransition;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int mana = 0;
    public int startHandSize = 5;
    public HealthBar healthBar;

    public TransitionSettings transition;

    public int startTurnMana = 0;

    private void Awake()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(GameManager.Instance.playerMaxHealth);
        }
        if (GameManager.Instance.playerHealth == 0)
        {
            GameManager.Instance.playerHealth = GameManager.Instance.playerMaxHealth;
        }

        healthBar.SetHealth(GameManager.Instance.playerHealth);
        startTurnMana = GameManager.Instance.playerStartTurnMana;
    }

    public void Heal(int amount)
    {
        GameManager.Instance.playerHealth = Math.Min(GameManager.Instance.playerMaxHealth, GameManager.Instance.playerHealth + amount);
        healthBar.SetHealth(GameManager.Instance.playerHealth);
    }

    public void TakeDamage(int amount)
    {

        int finalDamage = amount;
        foreach (var modifier in EffectManager.Instance.GetModifiers<IIncomingDamageModifier>())
        {
            finalDamage = modifier.ModifyIncomingDamage(finalDamage);
        }
        GameEvents.Instance.OnPlayerDamaged.Invoke(finalDamage);
        GameManager.Instance.playerHealth -= finalDamage;

        healthBar.SetHealth(GameManager.Instance.playerHealth);

        if (GameManager.Instance.playerHealth <= 0)
        {
            TransitionManager.Instance.Transition(5, transition, 0);
        }
    }

    public void GainMana(int amount)
    {
        mana += amount;
    }

    public void IncrementStartTurnMana()
    {
        startTurnMana = Math.Min(GameManager.Instance.playerMaxMana, startTurnMana + 1);
    }
}