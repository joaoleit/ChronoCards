using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        UpdateAnimation();
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    private void UpdateAnimation()
    {
        bool isMoving = move.magnitude > 0;
        animator.SetBool("IsWalking", isMoving);
    }
}
