using UnityEngine;
using System.Collections.Generic;
using EasyTransition;
using Unity.Behavior;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float enemyDifficulty { get; private set; } = 0.5f;
    public GameObject enemyThatAttacked;
    public Vector3 savedPlayerPosition;
    public EnemyType triggerEnemyType;
    public PlayerController player;
    public List<GameObject> objectsToDisable = new List<GameObject>();
    public bool isWorldActive = true;
    private bool isFirstBattle = true;
    private int previousBattleTurns = 0;
    public RewardsManager rewards;
    public SaveData currentSave;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        TransitionManager.Instance.onTransitionCutPointReached += () => ToggleElements(isWorldActive);
    }

    public void LoadSave(SaveData data)
    {
        currentSave = data;
        if (currentSave != null)
        {
            DeckManager.Instance.deck = currentSave.GetDeckCards();
            DeckManager.Instance.chest = currentSave.GetChestCards();
            enemyDifficulty = currentSave.enemyDifficulty;
            isFirstBattle = currentSave.enemyDifficulty == 0.5f;
        }
    }

    public void InitializeNewGame()
    {
        currentSave = new SaveData(new List<Card>(), new List<Card>(), Vector3.zero, 100f, new List<string>(), enemyDifficulty);
    }

    public void SaveCurrentGame()
    {
        List<Card> deck = DeckManager.Instance.deck;
        List<Card> chest = DeckManager.Instance.chest;
        Vector3 playerPosition = player.transform.position;
        float playerHealth = 100;

        SaveSystem.SaveGame(deck, chest, playerPosition, playerHealth, currentSave.defeatedEnemies, enemyDifficulty);
    }

    public void StartBattle(GameObject enemy)
    {
        Type[] componentsToDisable = new Type[]
            {
                typeof(BehaviorGraphAgent),
                typeof(TriggerBattleScene),
                typeof(EnemyAI),
            };

        foreach (Type componentType in componentsToDisable)
        {
            Component component = enemy.GetComponent(componentType);
            if (component != null) (component as Behaviour).enabled = false;
        }

        isWorldActive = false;
        enemyThatAttacked = enemy;
        savedPlayerPosition = player.transform.position;
        player.FreezePlayer(true);
    }

    public void OpenInventory()
    {
        player.FreezePlayer(true);
        isWorldActive = false;
    }

    public void CloseInventory()
    {
        isWorldActive = true;
        player.UnfreezePlayer();
    }

    public void EndBattle(bool enemyDefeated)
    {
        if (enemyDefeated && enemyThatAttacked != null)
        {
            isWorldActive = true;
            enemyThatAttacked.GetComponent<EnemyAI>().DefeatEnemy();
            Destroy(enemyThatAttacked);
            enemyThatAttacked = null;
            player.UnfreezePlayer();
        }
    }

    private void ToggleElements(bool active)
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj == null)
            {
                objectsToDisable.Remove(obj);
                continue;
            }
            obj.SetActive(active);
        }
        if (enemyThatAttacked == null) return;
        enemyThatAttacked?.SetActive(active);
    }

    public void SetTriggerEnemy(EnemyType enemyType)
    {
        triggerEnemyType = enemyType;
        Debug.Log($"TriggerEnemy definido como: {enemyType}");
    }

    public void CalculateDifficultyFactor()
    {
        if (isFirstBattle)
        {
            isFirstBattle = false;
            enemyDifficulty = 0.5f;
            return;
        }

        // Define parameters
        int minTurns = 1;      // Faster battles = higher increase
        int maxTurns = 20;     // Longer battles = lower increase
        float maxIncrease = 0.5f; // Maximum increase for very fast wins
        float minIncrease = 0.01f; // Minimum increase for slow battles
        float maxDifficulty = 5.0f; // Upper limit for difficulty

        // Clamp turnsTaken within expected range
        int turnsTaken = Mathf.Clamp(previousBattleTurns, minTurns, maxTurns);

        // Normalize turnsTaken (0 when fast, 1 when slow)
        float t = (turnsTaken - minTurns) / (float)(maxTurns - minTurns);

        // Inverse lerp: Higher increase when t is small (faster win), lower when t is large
        float difficultyIncrease = Mathf.Lerp(maxIncrease, minIncrease, t);

        // Increase difficulty instead of setting it
        enemyDifficulty = Mathf.Clamp(enemyDifficulty + difficultyIncrease, 0, maxDifficulty);
    }


    public void setBattleTurns(int turns)
    {
        previousBattleTurns = turns;
    }

    public void UpgradeCards()
    {
        foreach (Card card in DeckManager.Instance.deck)
        {
            card.UpgradeCard();
        }

        foreach (Card card in DeckManager.Instance.chest)
        {
            card.UpgradeCard();
        }
    }

    public Card CombineCards(Card card1, Card card2)
    {
        if (card1 == null || card2 == null) return null;

        string cardName = card1.cardName;
        int manaCost = Math.Min(card1.manaCost + card2.manaCost - 1, 10);
        List<ICardEffect> effects = new List<ICardEffect>();
        effects.AddRange(card1.effects);
        effects.AddRange(card2.effects);
        Color color = card2.color;
        Card card = CardFactory.CreateCard(cardName, manaCost, effects, color);

        return card;
    }

    public void GenerateRewards()
    {
        int deckAmount = DeckManager.Instance.chest.Count;
        int interestCards = Mathf.FloorToInt(deckAmount / 3);
        int total = interestCards + 1;

        var cards = StarterDeckCreator.getRandomCards(total);

        foreach (var card in cards)
        {
            DeckManager.Instance.AddCardToChest(card);
        }
        rewards.OpenPanel(interestCards, deckAmount, total);
    }
}
