using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public Card[] playerDeck;

    void Start()
    {
        // Initialize game state
    }

    public void PlayCard(int cardIndex)
    {
        if (cardIndex >= 0 && cardIndex < playerDeck.Length)
        {
            playerDeck[cardIndex].PlayCard(player, enemy);
        }
    }
}