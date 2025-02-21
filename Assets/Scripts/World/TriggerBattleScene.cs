using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

public class TriggerBattleScene : MonoBehaviour
{
    public string battleSceneName = "BattleScene";
    public PlayerController playerController;
    public Camera mainCamera;

    public EnemyType enemyType;

    public TransitionSettings transition;
    public float startDelay;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void StartBattleScene()
    {
        if (!string.IsNullOrEmpty(battleSceneName) && SceneManager.GetSceneByName(battleSceneName) != null && !GameManager.Instance.isBattleActive)
        {
            GameManager.Instance.StartBattle(gameObject, playerController);
            playerController.FreezePlayer(true);
            TransitionManager.Instance.Transition(battleSceneName, transition, startDelay);
        }
    }

}