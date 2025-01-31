using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int manaCost;
    public List<CardEffect> effects = new List<CardEffect>();

    public void PlayCard(Player player, Enemy enemy)
    {
        if (player.mana >= manaCost)
        {
            player.mana -= manaCost;
            foreach (var effect in effects)
            {
                effect.GetEffect().ApplyEffect(player, enemy);
            }
        }
        else
        {
            Debug.Log("Not enough mana!");
        }
    }
}