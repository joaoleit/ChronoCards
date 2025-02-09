using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private bool showDebugVisuals = true;


    public GameObject DetectedTarget {
        get;
        set;
    }

    public GameObject UpdateDetector() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        if (colliders.Length > 0)
        {
            DetectedTarget = colliders[0].gameObject;
            
        } else {
            DetectedTarget = null;
        }
        return DetectedTarget;
    }

    private void OnDrawGizmos()
    {
        if (showDebugVisuals)
        {
            // Define a cor do Gizmo
            Gizmos.color = Color.cyan;

            // Desenha um wireframe esférico para representar o raio de detecção
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            // Opcional: Desenha uma linha para o objeto detectado (se houver)
            GameObject detectedObject = UpdateDetector();
            if (detectedObject != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, detectedObject.transform.position);
            }
        }
    }
}
