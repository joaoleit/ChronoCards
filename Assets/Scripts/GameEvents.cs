using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    public UnityEvent OnTurnStart = new UnityEvent();
    public UnityEvent OnTurnEnd = new UnityEvent();
    public UnityEvent<Card> OnCardPlayed = new UnityEvent<Card>();
    public UnityEvent<IModifier> OnModifierAdded = new UnityEvent<IModifier>();
    public UnityEvent<IModifier> OnModifierExpired = new UnityEvent<IModifier>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
}