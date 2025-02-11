using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PersistentData", menuName = "Card Game/Persistent Data")]
public class PersistentDataManager : ScriptableObject
{
  [System.Serializable]
  public class SaveData
  {
    public Dictionary<string, int> chestCards = new Dictionary<string, int>();
    public List<string> deckCards = new List<string>();
  }

  public SaveData currentSave = new SaveData();
  private const string SAVE_KEY = "CardGameSave";

  public void Initialize()
  {
    LoadData();
  }

  public void SavePersistentData()
  {
    string json = JsonUtility.ToJson(currentSave);
    PlayerPrefs.SetString(SAVE_KEY, json);
    PlayerPrefs.Save();
  }

  public void LoadData()
  {
    if (PlayerPrefs.HasKey(SAVE_KEY))
    {
      currentSave = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));
    }
  }

  public void ResetData()
  {
    currentSave = new SaveData();
    SavePersistentData();
  }
}