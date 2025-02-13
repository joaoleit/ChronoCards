using UnityEngine;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager Instance;

    public EnemyType triggerEnemyType;

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Teste");


            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetTriggerEnemy(EnemyType enemyType)
    {
        this.triggerEnemyType = enemyType;
        Debug.Log($"TriggerEnemy definido como: {enemyType}");
    }
}