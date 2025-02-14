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

    public void UpgradeCard()
    {
        foreach (var effect in effects)
        {
            effect.value += 1; // Increase effect value by 1
            if (effect.effectType == CardEffect.EffectType.Passive && effect.passiveModifier != null)
            {
                effect.passiveModifier.value += 1; // Upgrade passive effect if applicable
            }
        }

        // Update description dynamically
        description = CardFactory.GenerateDescription(effects);
    }
}