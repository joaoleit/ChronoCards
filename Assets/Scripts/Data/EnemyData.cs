using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType; // Tipo do inimigo
    // public string enemyName; // Nome do inimigo
    // public int maxHealth; // Vida máxima
    // public int damage; // Dano do inimigo
}