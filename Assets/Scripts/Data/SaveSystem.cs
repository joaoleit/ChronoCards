using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    private static string savePath => $"{Application.persistentDataPath}/save.json";

    public static void SaveGame(
        List<Card> deck, 
        List<Card> chest, 
        Vector3 playerPosition, 
        int playerHealth, 
        List<string> defeatedEnemies, 
        float difficultyFactor,
        int healthMax, 
        int startTurnMana, 
        int maxMana, 
        int cardPerTurn
    )
    {
        SaveData saveData = new SaveData(
            deck, chest, playerPosition, playerHealth, defeatedEnemies, difficultyFactor, healthMax, startTurnMana, maxMana, cardPerTurn
        );
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to " + savePath);
    }

    public static SaveData LoadGame()
    {
        if (SaveExists())
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from " + savePath);
            return saveData;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + savePath);
            return null;
        }
    }

    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }

    public static void DeleteSave()
    {
        if (SaveExists()) File.Delete(savePath);
    }
}