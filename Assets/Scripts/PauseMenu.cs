using EasyTransition;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Assign in Inspector
    private bool isPaused = false;
    private bool isMuted = false;
    public TransitionSettings transition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuPanel.SetActive(false);
    }

    public void GoHome()
    {
        GameManager.Instance.SaveCurrentGame();
        isPaused = false;
        Time.timeScale = 1;
        TransitionManager.Instance.Transition(0, transition, 0);
    }

    public void QuitGame()
    {
        GameManager.Instance.SaveCurrentGame();
        Application.Quit();
    }
    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        UpdateMuteButtonText();
    }

    private void UpdateMuteButtonText()
    {
        // Text buttonText = muteButton.GetComponentInChildren<Text>();
        // buttonText.text = isMuted ? "Unmute" : "Mute";
    }
}
