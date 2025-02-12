using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadBattleScene()
    {
        int number = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(number);
    }
}
