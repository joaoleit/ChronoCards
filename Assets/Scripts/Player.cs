using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int mana = 10;

    public void Heal(int amount)
    {
        health += amount;
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