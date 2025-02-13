using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriggerBattleScene : MonoBehaviour
{
    public string battleSceneName = "BattleScene";
    public PlayerController playerController;
    public Camera mainCamera;

    public FadeInOutAnimationController fadeInOutAnimationController;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void StartBattleScene()
    {
        if (!string.IsNullOrEmpty(battleSceneName))
        {
            StartCoroutine(StartBattleSceneRoutine());
        }
        else
        {
            Debug.LogError("Nome da cena de batalha não foi definido!");
        }
    }

    private IEnumerator StartBattleSceneRoutine()
    {
        FreezePlayer();

        fadeInOutAnimationController.PlayFadeOut();

        yield return new WaitForSeconds(fadeInOutAnimationController.fadeOutDuration);

        SceneManager.LoadScene(battleSceneName);
    }

    public void FreezePlayer()
    {
        playerController.FreezePlayer(true);
    }
}