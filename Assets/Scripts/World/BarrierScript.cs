using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DeckManager.Instance.deck.Count != 0) {
            animator.SetTrigger("BarrierDown");
        }
    }
}
