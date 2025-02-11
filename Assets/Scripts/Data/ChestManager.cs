using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private PersistentDataManager dataManager;

    public void AddCardToChest(Card card)
    {
        if (dataManager.currentSave.chestCards.ContainsKey(card.cardName))
        {
            dataManager.currentSave.chestCards[card.cardName]++;
        }
        else
        {
            dataManager.currentSave.chestCards.Add(card.cardName, 1);
        }
        dataManager.SavePersistentData();
    }

    public int GetCardCount(string cardName)
    {
        return dataManager.currentSave.chestCards.TryGetValue(cardName, out int count) ? count : 0;
    }
}