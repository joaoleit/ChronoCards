using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text ManaText;
    public TMP_Text TurnText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMana(int mana)
    {
        ManaText.text = "Mana: " + mana;
    }

    public void UpdateTurn(string turn)
    {
        TurnText.text = turn + " turn";
    }
}
