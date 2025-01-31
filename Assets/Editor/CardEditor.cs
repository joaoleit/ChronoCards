using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Card card = (Card)target;

        // Draw default fields
        card.cardName = EditorGUILayout.TextField("Card Name", card.cardName);
        card.description = EditorGUILayout.TextField("Description", card.description);
        card.manaCost = EditorGUILayout.IntField("Mana Cost", card.manaCost);

        // Draw effects list
        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
        for (int i = 0; i < card.effects.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            card.effects[i].effectType = (CardEffect.EffectType)EditorGUILayout.EnumPopup("Effect Type", card.effects[i].effectType);
            card.effects[i].value = EditorGUILayout.IntField("Value", card.effects[i].value);
            if (GUILayout.Button("Remove Effect"))
            {
                card.effects.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Effect"))
        {
            card.effects.Add(new CardEffect());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(card);
        }
    }
}