using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyTime = 5f;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Color textColor = Color.red;

    private float horizontalSpeed;
    private float verticalSpeed;
    private float gravity = 5f;
    private float timeElapsed = 0f;

    private void Start()
    {
        transform.localPosition += offset;
        Destroy(gameObject, destroyTime);

        // Set random horizontal speed for variation (left or right)
        horizontalSpeed = Random.Range(-2f, 2f) * moveSpeed;
        verticalSpeed = moveSpeed * 2; // Initial upwards velocity
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Arch movement: horizontal movement + gravity effect
        float xMovement = horizontalSpeed * Time.deltaTime;
        float yMovement = (verticalSpeed - (gravity * timeElapsed)) * Time.deltaTime;

        transform.position += new Vector3(xMovement, yMovement, 0);
    }

    public void SetText(int damage)
    {
        GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();
    }
}
