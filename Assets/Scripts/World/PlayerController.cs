using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move;
    private Animator animator;
    private bool isFrozen = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isFrozen)
        {
            move = context.ReadValue<Vector2>();
        }
        else
        {
            move = Vector2.zero;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (GameManager.Instance != null && GameManager.Instance.savedPlayerPosition != Vector3.zero)
        {
            transform.position = GameManager.Instance.savedPlayerPosition;
        }
    }

    void Update()
    {
        movePlayer();
        UpdateAnimation();
    }

    public void movePlayer()
    {
        if (isFrozen) return;

        Vector3 movement = new Vector3(move.x, 0, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    private void UpdateAnimation()
    {
        bool isMoving = move.magnitude > 0 && !isFrozen;
        animator.SetBool("IsWalking", isMoving);
    }

    public void FreezePlayer(bool freeze)
    {
        isFrozen = freeze;
        if (freeze)
        {
            move = Vector2.zero;
            animator.SetBool("IsWalking", false); // Para a animação caso esteja congelado
        }
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
    }
}
