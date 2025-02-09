using UnityEngine;

public class LineOfSightDetector : MonoBehaviour
{
    [SerializeField] private float m_detectionHeight = 3.0f;
    [SerializeField] private float m_detectionRange = 10.0f;
    [SerializeField] private LayerMask m_playerLayerMask;
    [SerializeField] private bool showDebugVisuals = true;


    public GameObject PerformDetection(GameObject potentialTarget)
    {
        RaycastHit hit;
        Vector3 direction = potentialTarget.transform.position - transform.position;

        Physics.Raycast(transform.position + Vector3.up * m_detectionHeight, direction, out hit,
            m_detectionRange, m_playerLayerMask
        );

        if (hit.collider != null && hit.collider.gameObject == potentialTarget)
        {
            Debug.Log("potentialTarget is in line of sight!");
            if (showDebugVisuals && this.enabled)
            {
                Debug.DrawLine(transform.position + Vector3.up * m_detectionHeight, potentialTarget.transform.position, Color.red);
            }
            return hit.collider.gameObject;
        }
        else {
            return null;
        }
    }

    // public GameObject PerformDetection(GameObject potentialTarget)
    // {
    //     RaycastHit hit;
    //     Vector3 direction = potentialTarget.transform.position - transform.position;

    //     Physics.Raycast(transform.position, direction, out hit);

    //     if (Physics.Raycast(transform.position, direction, out hit))
    //     {
    //         if (hit.collider.gameObject == potentialTarget)
    //         {
    //             Debug.Log("potentialTarget is in line of sight!");
    //             return potentialTarget;
    //         }
    //     }
    //     return null;
    // }
}
