using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UpgradeManager : MonoBehaviour
{
    public UpgradeType upgradeType;
    [SerializeField] private GameObject _offerSlot;
    [SerializeField] private GameObject _combineGrid;
    [SerializeField] private GameObject _cardPrefab;

    public void setUpgrade(UpgradeType upgradeType)
    {
        this.upgradeType = upgradeType;
        _combineGrid.SetActive(upgradeType == UpgradeType.Forge);
    }

    public void BasicUpgrade()
    {
        if (!ValidateOffer()) return;

        List<GameObject> offerCard = GetOfferCard();
        switch (upgradeType)
        {
            case UpgradeType.Boost:
                GameManager.Instance.UpgradeCards();
                break;
            case UpgradeType.ManaCost:
                GameManager.Instance.UpgradeManaCost();
                break;
            case UpgradeType.Heal:
                GameManager.Instance.UpgradeHeal();
                break;
            case UpgradeType.IncreaseHealth:
                GameManager.Instance.UpgradeIncreaseHealth();
                break;
            case UpgradeType.IncreaseMaxMana:
                GameManager.Instance.UpgradeIncreaseMaxMana();
                break;
            case UpgradeType.IncreaseStartMana:
                GameManager.Instance.UpgradeIncreaseStartMana();
                break;
            case UpgradeType.IncreaseDraw:
                GameManager.Instance.UpgradeIncreaseDraw();
                break;
            default:
                Debug.LogWarning("Unknown upgrade type");
                break;
        }

        RemoveOldCardsFromSystem(offerCard);
        DestroyOldCards(offerCard);
        ClearUpgradeSlots();
        InventoryManager.Instance.InitializeSlots();
    }

    private bool ValidateOffer()
    {
        Slot offerSlot = _offerSlot.GetComponentInChildren<Slot>();

        return offerSlot.currentCard != null;
    }

    public List<GameObject> GetOfferCard()
    {
        Slot offerSlot = _offerSlot.GetComponentInChildren<Slot>();

        return new List<GameObject> { offerSlot.currentCard };
    }

    public void GetSlotsInUpgrade()
    {
        if (upgradeType != UpgradeType.Forge)
        {
            BasicUpgrade();
            return;
        }
        if (!ValidateSlots()) return;

        List<GameObject> involvedCards = GetInvolvedCards();
        Card combinedCard = CombineCards(involvedCards);

        if (!ValidateCombination(combinedCard))
        {
            ReturnCardsToOriginalSlots(involvedCards);
            ClearUpgradeSlots();
            return;
        }

        ProcessValidCombination(involvedCards, combinedCard);
    }

    private bool ValidateSlots()
    {
        Slot offerSlot = _offerSlot.GetComponentInChildren<Slot>();
        Slot[] combineGridSlots = _combineGrid.GetComponentsInChildren<Slot>();

        return offerSlot.currentCard != null &&
               combineGridSlots.All(s => s.currentCard != null);
    }

    public List<GameObject> GetInvolvedCards()
    {
        Slot offerSlot = _offerSlot.GetComponentInChildren<Slot>();
        Slot[] combineGridSlots = _combineGrid.GetComponentsInChildren<Slot>();

        List<GameObject> cards = new List<GameObject> { offerSlot.currentCard };
        cards.AddRange(combineGridSlots.Select(s => s.currentCard));
        return cards;
    }

    private Card CombineCards(List<GameObject> involvedCards)
    {
        List<Card> cards = involvedCards
            .Select(c => c.GetComponent<CardVisuals>().card)
            .ToList();

        return GameManager.Instance.CombineCards(cards[1], cards[2]);
    }

    private bool ValidateCombination(Card combinedCard)
    {
        return combinedCard != null && combinedCard.effects.Count < 4;
    }

    public void ReturnCardsToOriginalSlots(List<GameObject> cards)
    {
        foreach (GameObject card in cards)
        {
            if (card == null) continue;
            CardInventory cardInventory = card.GetComponent<CardInventory>();

            // Explicitly reset parent before returning
            card.transform.SetParent(cardInventory.originalSlot.transform);
            cardInventory.ReturnToOriginalSlot();
        }
    }

    public void ClearUpgradeSlots()
    {
        Slot offerSlot = _offerSlot.GetComponentInChildren<Slot>();
        Slot[] combineGridSlots = _combineGrid.GetComponentsInChildren<Slot>();

        offerSlot.currentCard = null;
        offerSlot.isOccupied = false;
        foreach (Slot slot in combineGridSlots)
        {
            slot.currentCard = null;
            slot.isOccupied = false;
        }
    }

    private void ProcessValidCombination(List<GameObject> oldCards, Card newCard)
    {
        Slot firstCombineSlot = _combineGrid.GetComponentsInChildren<Slot>()[0];
        InventoryType targetInventory = DetermineTargetInventory(oldCards);

        RemoveOldCardsFromSystem(oldCards);
        DestroyOldCards(oldCards);
        ClearUpgradeSlots();
        PlaceNewCard(newCard, targetInventory, firstCombineSlot);
    }

    private InventoryType DetermineTargetInventory(List<GameObject> cards)
    {
        return cards[0].GetComponent<CardInventory>().originalSlot.inventoryType;
    }

    private void RemoveOldCardsFromSystem(List<GameObject> cards)
    {
        foreach (GameObject card in cards)
        {
            Card cardData = card.GetComponent<CardVisuals>().card;
            DeckManager.Instance.RemoveCardFromDeck(cardData);
            DeckManager.Instance.RemoveCardFromChest(cardData);
        }
    }

    private void DestroyOldCards(List<GameObject> cards)
    {
        foreach (GameObject card in cards)
            Destroy(card);
    }

    private void PlaceNewCard(Card newCard, InventoryType targetInventory, Slot firstCombineSlot)
    {
        // Find appropriate inventory grid
        GameObject targetGrid = targetInventory == InventoryType.Deck
            ? InventoryManager.Instance._deckInventory
            : InventoryManager.Instance._chestInventory;

        if (targetInventory == InventoryType.Deck)
        {
            DeckManager.Instance.AddCardToDeck(newCard);
        }
        else
        {
            DeckManager.Instance.AddCardToChest(newCard);
        }
        // Find first available slot
        Slot targetSlot = targetGrid.GetComponentsInChildren<Slot>()
            .FirstOrDefault(s => !s.isOccupied);

        if (targetSlot != null)
        {
            GameObject newCardObj = Instantiate(_cardPrefab, targetSlot.transform);
            newCardObj.GetComponent<CardVisuals>().card = newCard;
            targetSlot.isOccupied = true;
            targetSlot.currentCard = newCardObj;
        }
    }
}