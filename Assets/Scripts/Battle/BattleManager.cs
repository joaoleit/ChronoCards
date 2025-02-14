using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using EasyTransition;

public class BattleManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public GameObject handObject;
    public GameObject cardPrefab;
    public GameObject basicEnemyPrefab;
    public GameObject selfHealingEnemyPrefab;
    public GameObject aggressiveEnemyPrefab;
    public List<Card> discardPile = new List<Card>();
    public int MaxHandSize = 10;
    private List<Card> deck;
    public static BattleManager Instance { get; private set; }
    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn
    }

    public TurnState currentTurn;
    private int turnCount = 0;
    private Vector3 enemyPosition = new Vector3(319.23f, 0, 30);

    public TransitionSettings transition;

    private void OnEnable()
    {
        GameEvents.Instance.OnEnemyDeath.AddListener(() =>
        {
            GameManager.Instance.EndBattle(true);
            GameManager.Instance.setBattleTurns(turnCount);
            TransitionManager.Instance.Transition(transition, 0);
        });
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        SpawnEnemy(enemyPosition);
        LoadAndShuffleDeck();
        player.mana = 0;
        StartBattle();
    }

    public void StartBattle()
    {
        DrawCards(player.startHandSize);
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        turnCount++;
        currentTurn = TurnState.PlayerTurn;
        player.IncrementStartTurnMana();
        player.mana = Math.Min(player.maxMana, player.startTurnMana);
        GameEvents.Instance.OnTurnStart.Invoke();
    }

    public void StartEnemyTurn()
    {
        currentTurn = TurnState.EnemyTurn;
        StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        // Calls the enemy's own turn logic, which now supports multiple moves and critical hits.
        yield return enemy.ExecuteTurn(player);
        yield return new WaitForSeconds(1f);
        StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            DrawCard();
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
            yield return new WaitForSeconds(0.3f);
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

    public void LoadAndShuffleDeck()
    {
        deck = new List<Card>(ShuffleDeck(DeckManager.Instance.deck));
        if (deck.Count == 0)
        {
            StarterDeckCreator.CreateStarterDeck();
            deck = new List<Card>(ShuffleDeck(DeckManager.Instance.deck));
        }
    }

    private List<Card> ShuffleDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        return deck;
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
        cardObject.transform.localPosition = Vector3.right * 10;
    }

    private IEnumerator AlignCardsNextFrame()
    {
        yield return null; // Wait for the next frame
        AlignCards();
    }

    // Spawns a new enemy at the specified position.
    public void SpawnEnemy(Vector3 spawnPosition)
    {
        // Calculate the difficulty factor based on turns taken.
        GameManager.Instance.CalculateDifficultyFactor();

        // Select the appropriate enemy prefab based on the difficulty factor.
        GameObject enemyPrefab = SelectEnemyPrefab(GameManager.Instance.enemyDifficulty);
        Debug.Log("Selected enemy prefab: " + enemyPrefab.name);
        if (enemyPrefab != null)
        {
            // Instantiate the enemy.
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Retrieve the Enemy component.
            Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // Set the difficulty factor and reinitialize the enemy's attributes.
                enemy = enemyScript;
                enemyScript.difficultyFactor = GameManager.Instance.enemyDifficulty;
                enemyScript.InitializeAttributes();  // Ensure attributes are updated immediately.
                Debug.Log("Spawned enemy with difficulty factor: " + GameManager.Instance.enemyDifficulty);
            }
        }
    }

    // Chooses an enemy prefab based on the difficulty factor.
    private GameObject SelectEnemyPrefab(float difficultyFactor)
    {
        if (difficultyFactor < 1.5f)
        {
            return basicEnemyPrefab;
        }
        else if (difficultyFactor < 2.5f)
        {
            return selfHealingEnemyPrefab;
        }
        else
        {
            return aggressiveEnemyPrefab;
        }
    }
}
