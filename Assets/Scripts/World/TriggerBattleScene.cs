using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

public class TriggerBattleScene : MonoBehaviour
{
    public string battleSceneName = "BattleScene";
    public TransitionSettings transition;
    public float startDelay;

    public void StartBattleScene()
    {
        if (!string.IsNullOrEmpty(battleSceneName) && SceneManager.GetSceneByName(battleSceneName) != null && GameManager.Instance.isWorldActive)
        {
            GameManager.Instance.StartBattle(gameObject);
            TransitionManager.Instance.Transition(battleSceneName, transition, startDelay);

            StartCoroutine(SwitchMusic());
        }
    }

    private IEnumerator SwitchMusic()
    {
        yield return new WaitForSeconds(2f);

        BackgroundMusic.Instance.SwitchToBattleMusic();
    }

}