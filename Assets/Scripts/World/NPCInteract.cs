using UnityEngine;
using TMPro;

public class NPCInteract : MonoBehaviour
{
    public float interactionRange = 3f; // Interaction distance
    private bool hasMetPlayer = false;
    private Transform player;

    [Header("UI Elements")]
    public GameObject dialogueBox; // Reference to the UI Panel
    public TMP_Text dialogueText; // Reference to the TMP Text

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dialogueBox.SetActive(false); // Hide dialogue initially
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange())
        {
            Interact();
        }
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= interactionRange;
    }

    void Interact()
    {
        if (!hasMetPlayer)
        {
            ShowDialogue("Welcome, traveler! Here, take this gift.");
            GiveItem();
            hasMetPlayer = true;
        }
        else
        {
            ShowDialogue("Ready to upgrade? Choose your option.");
            ShowUpgradeMenu();
        }
    }

    void ShowDialogue(string message)
    {
        dialogueText.text = message;
        dialogueBox.SetActive(true);
        Invoke("HideDialogue", 3f); // Auto-hide after 3 seconds
    }

    void HideDialogue()
    {
        dialogueBox.SetActive(false);
    }

    void GiveItem()
    {
        StarterDeckCreator.CreateStarterDeck();
        // Implement inventory system logic here
    }

    void ShowUpgradeMenu()
    {
        Debug.Log("Displaying upgrade menu...");
        // Implement upgrade logic here
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
