using UnityEngine;
using TMPro;

public class CardVisuals : MonoBehaviour
{
    public Card card;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text manaCostText;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        // Initialize visual elements from card data
        nameText.text = card.cardName;
        descriptionText.text = card.description;
        manaCostText.text = card.manaCost.ToString();
        _renderer.material.color = card.color;
    }
}