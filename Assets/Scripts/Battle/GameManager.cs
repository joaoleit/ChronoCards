using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public GameObject handObject;
    public GameObject cardPrefab;
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public int MaxHandSize = 10;

    public static GameManager Instance { get; private set; }
    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }

    public TurnState currentTurn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        player.mana = 0;
        DrawCards(3);
        StartPlayerTurn();
        // Initialize game state
        // LoadDeck();
    }

    public void StartPlayerTurn()
    {
        currentTurn = TurnState.PlayerTurn;
        player.IncrementStartTurnMana();
        player.mana = Math.Min(player.maxMana, player.startTurnMana);
        DrawCard();
        GameEvents.Instance.OnTurnStart.Invoke();
        Debug.Log("Player's turn");
    }

    public void StartEnemyTurn()
    {
        currentTurn = TurnState.EnemyTurn;
        StartCoroutine(EnemyAttackCoroutine());
    }

    private IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(1f);
        enemy.Attack(player);
        yield return new WaitForSeconds(1f);
        StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            StartEnemyTurn();
            GameEvents.Instance.OnTurnEnd.Invoke();
        }
    }

    public void PlayCard(CardDisplay cardDisplay)
    {
        Card card = cardDisplay.card;
        if (player.mana >= card.manaCost)
        {
            player.mana -= card.manaCost;
            card.PlayCard(player, enemy);
            Debug.Log("Played card: " + card.cardName);
            GameEvents.Instance.OnCardPlayed.Invoke(card);
            Destroy(cardDisplay.gameObject);
            discardPile.Add(card);
            StartCoroutine(AlignCardsNextFrame());
        }
        else
        {
            Debug.Log("Cannot play this card!");
        }
    }
    public void DrawCards(int count)
    {
        StartCoroutine(DrawCardsCoroutine(count));
    }

    public IEnumerator DrawCardsCoroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void DrawCard()
    {
        if (handObject.transform.childCount >= MaxHandSize)
        {
            Debug.Log("Hand is full!");
            return;
        }
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);

            // Instantiate the card display
            InstantiateCard(drawnCard);

            StartCoroutine(AlignCardsNextFrame());
            Debug.Log("Drew card: " + drawnCard.cardName);
        }
        else
        {
            Debug.Log("No cards left in the deck!");
        }
    }

    public void SaveDeck()
    {
        string path = Application.persistentDataPath + "/deck.json";
        List<string> deckJsonList = new List<string>();
        for (int i = 0; i < deck.Count; i++)
        {
            deckJsonList.Add(JsonUtility.ToJson(deck[i]));
        }
        File.WriteAllText(path, JsonUtility.ToJson(deckJsonList));
        Debug.Log("Deck saved to " + path);
    }

    public void LoadDeck()
    {
        string path = Application.persistentDataPath + "/deck.json";
        if (File.Exists(path))
        {
            string deckJson = File.ReadAllText(path);
            List<string> deckJsonList = JsonUtility.FromJson<List<string>>(deckJson);
            deck.Clear();
            foreach (string cardJson in deckJsonList)
            {
                Card card = ScriptableObject.CreateInstance<Card>();
                JsonUtility.FromJsonOverwrite(cardJson, card);
                deck.Add(card);
            }
            Debug.Log("Deck loaded from " + path);
        }
        else
        {
            Debug.Log("No saved deck found at " + path);
        }
    }

    void OnApplicationQuit()
    {
        SaveDeck();
    }

    public void AlignCards()
    {
        int cardCount = handObject.transform.childCount;
        float cardWidth = 1.5f; // Adjust this value based on your card width
        float gap = 0.1f; // Adjust this value to set the gap between cards
        float totalWidth = cardCount * cardWidth + (cardCount - 1) * gap;
        float startX = -(totalWidth - cardWidth) / 2;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject cardObject = handObject.transform.GetChild(i).gameObject;
            Vector3 cardNewPosition = new Vector3(startX + i * (cardWidth + gap), 0, 0);
            cardObject.GetComponent<CardHover>().setOriginalPosition(cardNewPosition);
        }
    }

    private void InstantiateCard(Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab, handObject.transform);
        cardObject.GetComponent<CardDisplay>().card = card;
        cardObject.GetComponent<CardDisplay>().manager = this;
        cardObject.transform.localPosition = Vector3.right * 10;
    }

    private IEnumerator AlignCardsNextFrame()
    {
        yield return null; // Wait for the next frame
        AlignCards();
    }
}
