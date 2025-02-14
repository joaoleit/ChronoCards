using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using EasyTransition;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float enemyDifficulty { get; private set; } = 1.0f;

    public GameObject enemyThatAttacked;
    public Vector3 savedPlayerPosition;

    public EnemyType triggerEnemyType;
    public PlayerController player;
    public GameObject worldCamera;
    public GameObject worldLight;
    private bool isBattleActive = false;

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
        if (worldCamera == null)
        {
            worldCamera = GameObject.FindWithTag("WorldCamera");
        }
        if (worldLight == null)
        {
            worldLight = GameObject.FindWithTag("WorldLight");
        }
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
        worldCamera.SetActive(active);
        worldLight.SetActive(active);
        enemyThatAttacked?.SetActive(active);
    }

    public void SetTriggerEnemy(EnemyType enemyType)
    {
        triggerEnemyType = enemyType;
        Debug.Log($"TriggerEnemy definido como: {enemyType}");
    }

    public void CalculateDifficultyFactor(int turnsTaken)
    {
        // Define the parameters.
        int minTurns = 1;      // Fewer turns = higher difficulty
        int maxTurns = 10;     // More turns = lower difficulty
        float maxFactor = 1.5f; // Highest difficulty factor when player wins fast
        float minFactor = 0.5f; // Lowest difficulty factor when the battle takes longer

        // Clamp turnsTaken to ensure it's within the expected range.
        turnsTaken = Mathf.Clamp(turnsTaken, minTurns, maxTurns);

        // Calculate a t value between 0 and 1.
        // t = 0 when turnsTaken == minTurns, t = 1 when turnsTaken == maxTurns.
        float t = (turnsTaken - minTurns) / (float)(maxTurns - minTurns);

        // Inverse lerp: when t is 0, we return maxFactor; when t is 1, we return minFactor.
        float difficultyFactor = Mathf.Lerp(maxFactor, minFactor, t);
        enemyDifficulty = difficultyFactor;
    }
}
