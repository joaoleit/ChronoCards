using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text ManaText;
    public TMP_Text TurnText;

    private void OnEnable()
    {
        GameEvents.Instance.OnCardPlayed.AddListener((Card card) => UpdateMana());
        GameEvents.Instance.OnTurnStart.AddListener(UpdateMana);
        GameEvents.Instance.OnTurnStart.AddListener(UpdateTurn);
        GameEvents.Instance.OnTurnEnd.AddListener(UpdateTurn);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMana()
    {
        int mana = BattleManager.Instance.player.mana;
        ManaText.text = "Mana: " + mana;
    }

    public void UpdateTurn()
    {
        string turn = BattleManager.Instance.currentTurn == BattleManager.TurnState.PlayerTurn ? "Player's" : "Enemy's";
        TurnText.text = turn + " turn";
    }
}
