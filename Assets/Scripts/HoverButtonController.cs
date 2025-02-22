using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private float fadeDuration = 0.2f;

    private CanvasGroup dialogCanvasGroup;

    void Start()
    {
        dialogBox.SetActive(false);
        dialogCanvasGroup = dialogBox.GetComponent<CanvasGroup>();
        if (dialogCanvasGroup == null) dialogCanvasGroup = dialogBox.AddComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        dialogBox.SetActive(true);
        StartCoroutine(FadeDialog(0, 1));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeDialog(1, 0, () => dialogBox.SetActive(false)));
    }

    IEnumerator FadeDialog(float startAlpha, float targetAlpha, System.Action onComplete = null)
    {
        float elapsed = 0;
        dialogCanvasGroup.alpha = startAlpha;

        while (elapsed < fadeDuration)
        {
            dialogCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dialogCanvasGroup.alpha = targetAlpha;
        onComplete?.Invoke();
    }
}