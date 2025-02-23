using TMPro;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text interestText;
    [SerializeField]
    private TMP_Text interestDescription;
    [SerializeField]
    private TMP_Text totalText;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private PlayerController player;

    void Awake()
    {
        GameManager.Instance.rewards = this;
    }

    public void OpenPanel(int interestValue, int deckAmount, int totalValue)
    {
        interestText.text = $"+{interestValue} card(s)";
        interestDescription.text = $"({deckAmount}) cards in chest";
        totalText.text = $"+{totalValue} card(s)";
        panel.SetActive(true);
        player.FreezePlayer(true);
    }

    public void ClosePanel()
    {
        player.UnfreezePlayer();
        panel.SetActive(false);
    }
}
