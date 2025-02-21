using UnityEngine;
using System.Collections.Generic;
using EasyTransition;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int deckSizeLimit = 25;

    [SerializeField]
    private GameObject _cardPrefab;
    public GameObject _chestInventory;
    public GameObject _deckInventory;
    public TransitionSettings transition;


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

    public void InitializeSlots()
    {
        Slot[] _chestSlots = _chestInventory.GetComponentsInChildren<Slot>();
        List<Card> chest = DeckManager.Instance.chest;
        for (int i = 0; i < _chestSlots.Length; i++)
        {
            Slot slot = _chestSlots[i];
            slot.isOccupied = false;
            slot.currentCard = null;
            if (slot.transform.childCount > 0) Destroy(slot.transform.GetChild(0).gameObject);
            slot.inventoryType = InventoryType.Chest;

            if (!(i < chest.Count)) continue;
            InstantiateCard(slot, chest[i]);
        }

        Slot[] _deckSlots = _deckInventory.GetComponentsInChildren<Slot>();
        List<Card> deck = DeckManager.Instance.deck;
        for (int i = 0; i < _deckSlots.Length; i++)
        {
            Slot slot = _deckSlots[i];
            slot.isOccupied = false;
            slot.currentCard = null;
            if (slot.transform.childCount > 0) Destroy(slot.transform.GetChild(0).gameObject);
            slot.inventoryType = InventoryType.Deck;

            if (!(i < deck.Count)) continue;
            InstantiateCard(slot, deck[i]);
        }
    }

    public bool IsValidTransfer(Slot from, Slot to)
    {
        if (to.isOccupied) return false;

        if (to.inventoryType == InventoryType.Combine)
        {
            Debug.Log(from.currentCard);
        }

        if (to.inventoryType == InventoryType.Deck)
        {
            return GetCurrentDeckCount() < deckSizeLimit;
        }
        return true;
    }

    int GetCurrentDeckCount()
    {
        int count = 0;
        foreach (Slot slot in _deckInventory.GetComponentsInChildren<Slot>())
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

    public void CloseInventory()
    {
        GameManager.Instance.CloseInventory();
        TransitionManager.Instance.Transition(transition, 0, "InventoryTesting");
    }
}

public enum InventoryType
{
    Chest,
    Deck,
    Offer,
    Combine
}