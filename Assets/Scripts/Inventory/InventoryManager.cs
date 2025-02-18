using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int deckSizeLimit = 5;

    private List<Slot> chestSlots = new List<Slot>();
    private List<Slot> deckSlots = new List<Slot>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSlots();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeSlots()
    {
        Slot[] allSlots = FindObjectsByType<Slot>(FindObjectsSortMode.None);
        foreach (Slot slot in allSlots)
        {
            if (slot.inventoryType == InventoryType.Chest)
                chestSlots.Add(slot);
            else
                deckSlots.Add(slot);
        }
    }

    public bool IsValidTransfer(Slot from, Slot to)
    {
        if (to.isOccupied) return false;

        if (to.inventoryType == InventoryType.Deck)
        {
            return GetCurrentDeckCount() < deckSizeLimit;
        }
        return true;
    }

    int GetCurrentDeckCount()
    {
        int count = 0;
        foreach (Slot slot in deckSlots)
        {
            if (slot.isOccupied) count++;
        }
        return count;
    }
}

public enum InventoryType
{
    Chest,
    Deck
}