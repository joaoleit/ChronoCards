using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // if (player != null)
        // {
        //     agent.SetDestination(player.position);
        // }

        float speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", speed);
    }
}