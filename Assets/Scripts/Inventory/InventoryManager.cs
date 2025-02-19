using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int deckSizeLimit = 25;

    private List<Slot> chestSlots = new List<Slot>();
    private List<Slot> deckSlots = new List<Slot>();
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private GameObject _chestInventory;
    [SerializeField]
    private GameObject _deckInventory;

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
        Slot[] _chestSlots = _chestInventory.GetComponentsInChildren<Slot>();
        List<Card> chest = DeckManager.Instance.chest;
        for (int i = 0; i < _chestSlots.Length; i++)
        {
            Slot slot = _chestSlots[i];
            chestSlots.Add(slot);
            slot.isOccupied = false;
            slot.inventoryType = InventoryType.Chest;

            if (!(i < chest.Count)) continue;
            InstantiateCard(slot, chest[i]);
        }

        Slot[] _deckSlots = _deckInventory.GetComponentsInChildren<Slot>();
        List<Card> deck = DeckManager.Instance.deck;
        for (int i = 0; i < _deckSlots.Length; i++)
        {
            Slot slot = _deckSlots[i];
            deckSlots.Add(slot);
            slot.inventoryType = InventoryType.Deck;

            if (!(i < deck.Count)) continue;
            InstantiateCard(slot, deck[i]);
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

    private void InstantiateCard(Slot slot, Card card)
    {
        GameObject cardObject = Instantiate(_cardPrefab, slot.transform);
        cardObject.GetComponent<CardVisuals>().card = card;
        slot.isOccupied = true;
    }
}

public enum InventoryType
{
    Chest,
    Deck,
    Upgrade
}