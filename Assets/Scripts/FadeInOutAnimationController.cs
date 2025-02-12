using UnityEngine;

public class FadeInOutAnimationController : MonoBehaviour
{
    private Animator animator;
    public float fadeOutDuration = 2.5f;
    public float fadeInDuration = 2.5f;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Idle"); 
        }
    }

    public void PlayFadeIn()
    {
        if (animator != null)
        {
            animator.SetTrigger("FadeIn");
        }
    }

    public void PlayFadeOut()
    {
        if (animator != null)
        {
            animator.SetTrigger("FadeOut");
        }
    }
}
