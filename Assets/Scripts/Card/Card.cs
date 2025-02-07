using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int manaCost;
    public List<CardEffect> effects = new List<CardEffect>();
    public Color color;

    public void PlayCard(Player player, Enemy enemy)
    {
        foreach (var effect in effects)
        {
            effect.GetEffect().ApplyEffect(player, enemy);
        }
    }
}