using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    public string ID;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (GameManager.Instance.currentSave.defeatedEnemies.Contains(ID))
        {
            Destroy(gameObject);
        }
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

    public void DefeatEnemy()
    {
        if (!GameManager.Instance.currentSave.defeatedEnemies.Contains(ID))
        {
            GameManager.Instance.currentSave.defeatedEnemies.Add(ID);
            GameManager.Instance.SaveCurrentGame();
        }
    }
}