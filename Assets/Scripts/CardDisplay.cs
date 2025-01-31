using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public new TMP_Text name;
    public TMP_Text description;
    public TMP_Text manaCost;
    public GameManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        name.text = card.cardName;
        description.text = card.description;
        manaCost.text = card.manaCost.ToString();
        gameObject.GetComponent<Renderer>().material.color = card.color;
    }

    private void OnMouseDown()
    {
        manager.PlayCard(card);
        //Destroy(this);
    }

}
