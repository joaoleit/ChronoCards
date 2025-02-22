using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int manaCost;
    public List<ICardEffect> effects = new List<ICardEffect>();
    public Color color;

    public void PlayCard(Player player, Enemy enemy)
    {
        foreach (var effect in effects)
        {
            effect.ApplyEffect(player, enemy);
        }
    }

    public void UpgradeCard()
    {
        foreach (var effect in effects)
        {
            effect.UpgradeEffect();
        }

        description = CardFactory.GenerateDescription(effects);
    }
}