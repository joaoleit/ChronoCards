using UnityEngine;
using UnityEngine.AI;

public class BarrierScript : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(DeckManager.Instance.deck.Count != 0) {
            animator.SetTrigger("BarrierDown");
            Transform obstacleTransform = transform.Find("Obstacle");
            
            if (obstacleTransform != null)
            {
                NavMeshObstacle navMeshObstacle = obstacleTransform.GetComponent<NavMeshObstacle>();
                if (navMeshObstacle != null)
                {
                    navMeshObstacle.enabled = false;
                }
            }
        }
    }
}
