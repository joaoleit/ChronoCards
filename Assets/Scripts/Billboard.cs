using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;
    void Start()
    {
        if (!cam)
        {
            cam = Camera.main.transform;
        }
        transform.LookAt(transform.position + cam.forward);
    }

}
