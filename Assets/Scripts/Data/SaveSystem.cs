using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    private static string savePath => $"{Application.persistentDataPath}/save.dat";

    public static void SaveGame(SaveData data)
    {
        Debug.Log(savePath);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                return (SaveData)formatter.Deserialize(stream);
            }
        }
        return null;
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