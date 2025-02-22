using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyTransition;

public class BattleManager : MonoBehaviour
{
    public Player player;
    public List<Enemy> enemies = new List<Enemy>();
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
    private Vector3 baseEnemyPosition = new Vector3(319.23f, 0, 28f);
    public TransitionSettings transition;

    private void OnEnable()
    {
        GameEvents.Instance.OnEnemyTurnStart.AddListener(StartEnemyTurn);
        GameEvents.Instance.OnTurnStart.AddListener(StartPlayerTurn);

        GameEvents.Instance.OnEnemyDeath.AddListener(() =>
        {
            foreach (var e in enemies)
            {
                if (e != null && e.health >= 0) return;
            }
            GameManager.Instance.EndBattle(true);
            GameManager.Instance.setBattleTurns(turnCount);
            TransitionManager.Instance.Transition(transition, 0, "BattleScene");
            StartCoroutine(SwitchMusic());
        });

    }

    private IEnumerator SwitchMusic()
    {
        yield return new WaitForSeconds(2f);

        BackgroundMusic.Instance.SwitchToWorldMusic();
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
        SpawnEnemies();
        LoadAndShuffleDeck();
        player.mana = 0;
        StartBattle();
    }

    public void StartBattle()
    {
        DrawCards(player.startHandSize);
        GameEvents.Instance.OnTurnStart.Invoke();
    }

    public void StartPlayerTurn()
    {
        turnCount++;
        currentTurn = TurnState.PlayerTurn;
        player.IncrementStartTurnMana();
        player.mana = Math.Min(player.maxMana, player.startTurnMana);
    }

    public void StartEnemyTurn()
    {
        currentTurn = TurnState.EnemyTurn;
        StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        // Calls the enemy's own turn logic, which now supports multiple moves and critical hits.
        foreach (var enemy in enemies)
        {
            if (enemy.health <= 0) continue;
            yield return enemy.ExecuteTurn(player);
            yield return new WaitForSeconds(1f);
        }
        GameEvents.Instance.OnEnemyTurnEnd.Invoke();
    }

    public void EndPlayerTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            DrawCard();
            GameEvents.Instance.OnTurnEnd.Invoke();
        }
    }

    public bool PlayCard(CardLogic cardLogic, Enemy enemy)
    {
        Card card = cardLogic._card;
        if (player.mana >= card.manaCost)
        {
            player.mana -= card.manaCost;
            card.PlayCard(player, enemy);
            GameEvents.Instance.OnCardPlayed.Invoke(card);
            Destroy(cardLogic.gameObject);
            discardPile.Add(card);
            StartCoroutine(AlignCardsNextFrame());
            
            foreach (var effect in card.effects)
            {
                if (effect is IPlayAudioEffect)
                {
                    IPlayAudioEffect audioEffect = effect as IPlayAudioEffect;
                    AudioManager.Instance.Play(audioEffect.GetAudioName().ToString());
                }
            }
            return true;
        }
        PopUpManager.Instance.InstantiatePopUp("Not enough Mana");
        return false;
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

            AudioManager.Instance.Play("DrawCard");

            StartCoroutine(AlignCardsNextFrame());
        }
        else
        {
            PopUpManager.Instance.InstantiatePopUp("No cards left in the deck!");
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
        cardObject.GetComponent<CardVisuals>().card = card;
        cardObject.transform.localPosition = Vector3.right * 10;
    }

    private IEnumerator AlignCardsNextFrame()
    {
        yield return null; // Wait for the next frame
        AlignCards();
    }

    private void SpawnEnemies()
    {
        GameManager.Instance.CalculateDifficultyFactor();
        float totalDifficulty = GameManager.Instance.enemyDifficulty;
        int numberOfEnemies = 1;//UnityEngine.Random.Range(1, 4); // 1, 2, or 3 enemies

        // Select the appropriate enemy prefab based on the difficulty factor.
        GameObject enemyPrefab = GameManager.Instance.enemyThatAttacked;
        Debug.Log($"Number of enemies: {numberOfEnemies}");

        float partialDifficulty = totalDifficulty / numberOfEnemies;

        float spacing = 7f;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPos = CalculateEnemyPosition(i, numberOfEnemies, spacing);
            float part = UnityEngine.Random.Range(0.8f, 1.2f);
            float enemyDifficulty = partialDifficulty * part;
            SpawnEnemy(spawnPos, enemyDifficulty, enemyPrefab);
        }
    }

    private Vector3 CalculateEnemyPosition(int index, int totalEnemies, float spacing)
    {
        float xOffset = (index - (totalEnemies - 1) / 2f) * spacing;
        return baseEnemyPosition + new Vector3(xOffset, 0, 0);
    }

    private Enemy SelectEnemyScript(GameObject enemy, float difficultyFactor)
    {
        Enemy enemyScript;
        if (difficultyFactor < 1.5f)
        {
            enemyScript = enemy.GetComponent<Enemy>();
        }
        else if (difficultyFactor < 2.5f)
        {
            enemyScript = enemy.GetComponent<SelfHealingEnemy>();
        }
        else
        {
            enemyScript = enemy.GetComponent<AggressiveEnemy>();
        }

        return enemyScript;
    }

    private void SpawnEnemy(Vector3 spawnPosition, float difficulty, GameObject enemyPrefab)
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemyInstance.SetActive(true);
        // Store the original X rotation
        float originalXRotation = enemyInstance.transform.eulerAngles.x;

        // Make the enemy face the camera
        enemyInstance.transform.LookAt(Camera.main.transform);

        // Keep the original X rotation, but update Y and Z to face the camera
        Vector3 newRotation = enemyInstance.transform.eulerAngles;
        newRotation.x = originalXRotation; // Restore original X rotation
        enemyInstance.transform.eulerAngles = newRotation;

        Transform enemyCanvasTransform = enemyInstance.transform.Find("EnemyCanvas");
        if (enemyCanvasTransform != null)
        {
            enemyCanvasTransform.gameObject.SetActive(true);
        }

        Enemy enemyScript = SelectEnemyScript(enemyInstance, difficulty);
        if (enemyScript != null)
        {
            enemyScript.enabled = true;
            enemyScript.difficultyFactor = difficulty;
            enemyScript.InitializeAttributes();
            enemies.Add(enemyScript);
            Debug.Log("Spawned enemy with difficulty: " + difficulty);
        }
    }
}
