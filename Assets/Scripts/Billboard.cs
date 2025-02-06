using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;
    void Start()
    {
        transform.LookAt(transform.position + cam.forward);
    }

}
