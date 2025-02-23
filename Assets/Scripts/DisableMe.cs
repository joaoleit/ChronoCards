using UnityEngine;

public class DisableMe : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.objectsToDisable.Add(gameObject);
    }
}
