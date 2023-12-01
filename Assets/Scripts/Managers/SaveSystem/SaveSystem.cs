using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData(string profileName)
    {
        SaveData saveData = new SaveData()
        {
            Name = profileName,
            DefeatedEnemiesAllTime = 0,
            RecivedEndings = null
        };
        string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";

        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(dataPath, jsonString);

        Debug.LogWarning("New Profile: " + profileName);
    }

    public static void SaveData(string profileName, int defeatedEnemies, EndingData obtainedEnding)
    {
        SaveData savedData = LoadData(profileName);
        if (savedData != null)
        {
            savedData.DefeatedEnemiesAllTime += defeatedEnemies;
            savedData.RecivedEndings.Add(obtainedEnding);

            string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";

            string jsonString = JsonUtility.ToJson(savedData);
            File.WriteAllText(dataPath, jsonString);

            Debug.LogWarning("Saved Data For: " + savedData.Name);
        }
    }

    public static void SaveData(SaveData dataToSave)
    {
        SaveData savedData = LoadData(dataToSave);
        if (savedData != null)
        {
            savedData.DefeatedEnemiesAllTime += dataToSave.DefeatedEnemiesAllTime;

            foreach(EndingData ending in dataToSave.RecivedEndings)
            {
                savedData.RecivedEndings.Add(ending);
            }

            string dataPath = Application.persistentDataPath + $"/{dataToSave.Name}_sav.json";

            string jsonString = JsonUtility.ToJson(savedData);
            File.WriteAllText(dataPath, jsonString);

            Debug.LogWarning("Saved Data For: " + savedData.Name);
        }
    }

    public static SaveData LoadData(string profileName)
    {
        string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";
        if(File.Exists(dataPath))
        {
            string jsonString = File.ReadAllText(dataPath);
            SaveData savedData = JsonUtility.FromJson<SaveData>(jsonString);
            Debug.LogWarning("Data loaded for: " + savedData.Name);
            return savedData;
        }
        else
        {
            Debug.LogError("Data not found for: " + profileName);
            return null;
        }
    }

    public static SaveData LoadData(SaveData dataToLoad)
    {
        string dataPath = Application.persistentDataPath + $"/{dataToLoad.Name}_sav.json";
        if (File.Exists(dataPath))
        {
            string jsonString = File.ReadAllText(dataPath);
            SaveData savedData = JsonUtility.FromJson<SaveData>(jsonString);
            Debug.LogWarning("Data loaded for: " + savedData.Name);
            return savedData;
        }
        else
        {
            Debug.LogError("Data not found for: " + dataToLoad.Name);
            return null;
        }
    }
}
