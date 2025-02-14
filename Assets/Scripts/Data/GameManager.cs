using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using EasyTransition;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float enemyDifficulty { get; private set; } = 0.5f;

    public GameObject enemyThatAttacked;
    public Vector3 savedPlayerPosition;

    public EnemyType triggerEnemyType;
    public PlayerController player;
    public List<GameObject> objectsToDisable;
    private bool isBattleActive = false;
    private bool isFirstBattle = true;
    private int previousBattleTurns = 0;

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
        TransitionManager.Instance.onTransitionCutPointReached += () => ToggleElements(!isBattleActive);
    }
    public void StartBattle(GameObject enemy, PlayerController player)
    {
        isBattleActive = true;
        enemyThatAttacked = enemy;
        this.player = player;
        savedPlayerPosition = player.transform.position;
    }

    public void EndBattle(bool enemyDefeated)
    {
        if (enemyDefeated && enemyThatAttacked != null)
        {
            isBattleActive = false;
            Destroy(enemyThatAttacked);
            enemyThatAttacked = null;
            player.UnfreezePlayer();
        }
    }

    private void ToggleElements(bool active)
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(active);
        }
        enemyThatAttacked.SetActive(active);
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
}
