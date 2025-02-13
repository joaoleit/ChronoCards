using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    void Start()
    {
        if (CustomSceneManager.Instance != null)
        {
            EnemyType enemyType = CustomSceneManager.Instance.triggerEnemyType;

            Debug.Log($"Inimigo que ativou a batalha: {enemyType}");

            SpawnEnemy(enemyType);
        }
        else
        {
            Debug.LogError("CustomSceneManager não foi encontrado!");
        }
    }

    void SpawnEnemy(EnemyType enemyType)
    {
        // Lógica para instanciar o inimigo com base no tipo
        Debug.Log($"Instanciando inimigo do tipo: {enemyType}");
        // Exemplo: Instantiate(prefabDoInimigo, posição, rotação);
    }
}