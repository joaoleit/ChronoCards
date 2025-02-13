using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    public List<Card> chest = new List<Card>();
    public List<Card> deck = new List<Card>();
    public const int MaxDeckSize = 30;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddCardToDeck(Card card)
    {
        if (deck.Count < MaxDeckSize)
        {
            deck.Add(card);
        }
    }

    public void AddCardToChest(Card card)
    {
        chest.Add(card);
    }


    void OnApplicationQuit()
    {
        SaveDeck();
    }

    public void SaveDeck()
    {
        Debug.Log("Saving deck");
        string deckJson = JsonUtility.ToJson(deck);
        PlayerPrefs.SetString("PlayerDeck", deckJson);
        PlayerPrefs.Save();
    }
}
