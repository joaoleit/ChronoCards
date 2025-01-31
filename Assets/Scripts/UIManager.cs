using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public Button[] cardButtons;

    void Start()
    {
        for (int i = 0; i < cardButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            cardButtons[i].onClick.AddListener(() => gameManager.PlayCard(index));
        }
    }
}