using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class NPCInteract : MonoBehaviour
{
    public float interactionRange = 3f;
    private bool hasMetPlayer = false;
    private PlayerController player;

    [Header("UI Elements")]
    public GameObject dialogueBox;
    public GameObject exclamationBox;
    public GameObject pressEBox;
    public TMP_Text dialogueText;
    public TriggerInventoryScene trigger;
    public List<string> dialog = new List<string>();

    private int currentDialogueIndex;
    private bool isInDialog;
    private Coroutine dialogueRoutine;
    public float inputCooldown = 0.2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        dialogueBox.SetActive(false);
        pressEBox.SetActive(false);
        hasMetPlayer = DeckManager.Instance.deck.Count + DeckManager.Instance.chest.Count > 0;
    }

    void Update()
    {
        if (isInDialog) return;

        if (IsPlayerInRange())
        {
            pressEBox.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
        else
        {
            pressEBox.SetActive(false);
        }
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= interactionRange;
    }

    void Interact()
    {
        if (!hasMetPlayer)
        {
            StartDialogue();
            hasMetPlayer = true;
        }
        else
        {
            ShowUpgradeMenu();
        }
    }

    void StartDialogue()
    {
        player.FreezePlayer(true);
        currentDialogueIndex = 0;
        isInDialog = true;
        dialogueRoutine = StartCoroutine(RunDialogueSequence());
    }

    IEnumerator RunDialogueSequence()
    {
        // Wait for E key to be released first
        while (Input.GetKey(KeyCode.E))
        {
            yield return null;
        }

        while (currentDialogueIndex < dialog.Count)
        {
            dialogueText.text = dialog[currentDialogueIndex];
            dialogueBox.SetActive(true);
            exclamationBox.SetActive(false);

            // Wait for new E press
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

            // Advance to next line
            currentDialogueIndex++;

            // Wait for cooldown and key release
            yield return new WaitForSeconds(inputCooldown);
            while (Input.GetKey(KeyCode.E))
            {
                yield return null;
            }
        }

        HideDialogue();
    }

    void HideDialogue()
    {
        dialogueBox.SetActive(false);
        exclamationBox.SetActive(true);
        player.FreezePlayer(false);
        isInDialog = false;
        GiveItem();
    }

    void GiveItem()
    {
        StarterDeckCreator.CreateStarterDeck();
    }

    void ShowUpgradeMenu()
    {
        trigger.StartInventoryScene();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}