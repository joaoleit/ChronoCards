// using UnityEngine;

// public enum EnemyState
// {
//     Patrolling,
//     Chasing,
//     Attacking
// }

// public class EnemyAI : MonoBehaviour
// {
//     public Transform player;
//     public float chaseDistance = 7f;
//     public float attackDistance = 2f;
//     public float speed = 3f;

//     private EnemyState currentState;
//     private Animator animator;

//     void Start()
//     {
//         currentState = EnemyState.Patrolling;
//         animator = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         switch (currentState)
//         {
//             case EnemyState.Patrolling:
//                 Patrol();
//                 if (distanceToPlayer <= chaseDistance)
//                 {
//                     currentState = EnemyState.Chasing;
//                 }
//                 break;

//             case EnemyState.Chasing:
//                 Chase();
//                 if (distanceToPlayer <= attackDistance)
//                 {
//                     currentState = EnemyState.Attacking;
//                 }
//                 else if (distanceToPlayer > chaseDistance)
//                 {
//                     currentState = EnemyState.Patrolling;
//                 }
//                 break;

//             case EnemyState.Attacking:
//                 Attack();
//                 if (distanceToPlayer > attackDistance)
//                 {
//                     currentState = EnemyState.Chasing;
//                 }
//                 break;
//         }

//         UpdateAnimation();
//     }

//     void Patrol()
//     {
//         Debug.Log("Patrulhando...");
//         // Lógica de patrulha (por exemplo, mover-se entre waypoints)
//         // Aqui você pode adicionar a lógica de movimento durante a patrulha
//     }

//     void Chase()
//     {
//         Debug.Log("Perseguindo o jogador...");
//         Vector3 direction = (player.position - transform.position).normalized;
//         transform.position += direction * speed * Time.deltaTime;
//         transform.LookAt(player);
//     }

//     void Attack()
//     {
//         Debug.Log("Atacando o jogador!");
//         // Lógica de ataque (por exemplo, causar dano ao jogador)
//     }

//     void UpdateAnimation()
//     {
//         // bool isMoving = currentState == EnemyState.Patrolling || currentState == EnemyState.Chasing;
//         bool isMoving = currentState == EnemyState.Chasing;
//         animator.SetBool("IsWalking", isMoving);
//     }
// }