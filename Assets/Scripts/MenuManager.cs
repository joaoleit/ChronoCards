using UnityEngine;
using EasyTransition;

public class MenuManager : MonoBehaviour
{
    public GameObject continueButton; // Assign in Inspector
    public TransitionSettings transition;

    void Start()
    {
        // Check for existing save
        continueButton.SetActive(SaveSystem.SaveExists());
    }

    public void ContinueGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            GameManager.Instance.LoadSave(data);
            TransitionManager.Instance.Transition(1, transition, 0);
        }
    }

    public void NewGame()
    {
        SaveSystem.DeleteSave();
        GameManager.Instance.InitializeNewGame();
        TransitionManager.Instance.Transition(1, transition, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
