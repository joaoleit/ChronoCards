using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Card data
    public List<Card> deck = new List<Card>();
    public List<Card> chest = new List<Card>();

    // Player state
    public SerializableVector3 playerPosition;
    public float playerHealth;

    // Game progress
    public List<string> defeatedEnemies = new List<string>();
}

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}