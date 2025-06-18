using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataPersistence 
{

    public  const string Key = "Data";

    public static void SaveDataToDisk(string dataJson) 
    {

      PlayerPrefs.SetString(Key, dataJson);
        Debug.Log("Data Saved");
    }
    public static GameData LoadDataFromDisk() 
    {
        if (PlayerPrefs.HasKey(Key)) 
        {
            GameData cardDataInstance = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(Key));
            Debug.Log("update data Loaded from disk");
            return cardDataInstance;

        }
        Debug.Log("update data Loaded from disk is null");
        return null;
    }

    public static void DeleteData() 
    {
      PlayerPrefs.DeleteKey(Key);
    
    }

}
