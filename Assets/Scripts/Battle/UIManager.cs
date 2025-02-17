using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TMP_Text ManaText;
    public Image playerImage;          // The image on your canvas that indicates the player's turn
    public Image enemyImage;          // The image on your canvas that indicates the player's turn
    public float growDuration = 0.1f;  // Duration for the image to grow
    public float waitTime = 0.5f;       // Time to wait after growing before fading out
    public float fadeDuration = 0.1f;  // Duration for the image to fade out
    public Vector3 finalScale = Vector3.one;  // The final scale (e.g., 1,1,1)

    private CanvasGroup playerCanvasGroup;
    private CanvasGroup enemyCanvasGroup;

    private void OnEnable()
    {
        GameEvents.Instance.OnCardPlayed.AddListener((Card card) => UpdateMana());
        GameEvents.Instance.OnTurnStart.AddListener(UpdateMana);

        GameEvents.Instance.OnEnemyTurnEnd.AddListener(StartPlayerTurnAnimation);
        GameEvents.Instance.OnTurnEnd.AddListener(StartEnemyTurnAnimation);
    }

    void Start()
    {
        // Ensure we have a CanvasGroup for fading
        playerCanvasGroup = playerImage.GetComponent<CanvasGroup>();
        if (playerCanvasGroup == null)
        {
            playerCanvasGroup = playerImage.gameObject.AddComponent<CanvasGroup>();
        }

        // Start with the image hidden (scaled down and fully opaque)
        playerImage.transform.localScale = Vector3.zero;
        playerCanvasGroup.alpha = 1f;

        // Ensure we have a CanvasGroup for fading
        enemyCanvasGroup = enemyImage.GetComponent<CanvasGroup>();
        if (enemyCanvasGroup == null)
        {
            enemyCanvasGroup = enemyImage.gameObject.AddComponent<CanvasGroup>();
        }

        // Start with the image hidden (scaled down and fully opaque)
        enemyImage.transform.localScale = Vector3.zero;
        enemyCanvasGroup.alpha = 1f;
    }

    public void UpdateMana()
    {
        int mana = BattleManager.Instance.player.mana;
        ManaText.text = "Mana: " + mana;
    }

    // Call this method to trigger the turn animation
    public void StartPlayerTurnAnimation()
    {
        // In case an animation is already running, stop it first
        // StopAllCoroutines();
        StartCoroutine(AnimateTurn(playerImage, playerCanvasGroup, true));
    }

    // Call this method to trigger the turn animation
    public void StartEnemyTurnAnimation()
    {
        // In case an animation is already running, stop it first
        // StopAllCoroutines();
        StartCoroutine(AnimateTurn(enemyImage, enemyCanvasGroup, false));
    }

    IEnumerator AnimateTurn(Image turnImage, CanvasGroup canvasGroup, bool isPlayer)
    {
        // --- Grow the Image ---
        float timer = 0f;
        while (timer < growDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / growDuration);
            turnImage.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, progress);
            yield return null;
        }
        // Ensure final scale is set
        turnImage.transform.localScale = finalScale;

        // --- Wait for a moment ---
        yield return new WaitForSeconds(waitTime);

        // --- Fade Out the Image ---
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return null;
        }
        // Ensure fully faded out
        canvasGroup.alpha = 0f;
        turnImage.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;

        if (isPlayer)
        {
            GameEvents.Instance.OnTurnStart.Invoke();
        }
        else
        {
            GameEvents.Instance.OnEnemyTurnStart.Invoke();

        }
    }
}
