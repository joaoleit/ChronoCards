using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCooldown : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Button targetButton; // Assign in Inspector
    [SerializeField] private float cooldownTime = 3f; // Cooldown duration

    private void Start()
    {
        // Ensure the button is assigned
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

        // Add click listener
        targetButton.onClick.AddListener(StartCooldown);
    }

    private void StartCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        // Disable the button
        targetButton.interactable = false;

        // Wait for cooldown
        yield return new WaitForSeconds(cooldownTime);

        // Re-enable the button
        targetButton.interactable = true;
    }
}