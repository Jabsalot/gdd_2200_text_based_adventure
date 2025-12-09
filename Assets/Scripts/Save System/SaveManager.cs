using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages saving and loading game data to/from disk.
/// </summary>
public static class SaveManager
{
    private static readonly string FilePath =
        Application.persistentDataPath + "/GameSaveData.json";

    public static void Save(GameData data)
    {
        TrySave(data);
    }

    private static void TrySave(GameData data)
    {
        try
        {
            // Record game version + save timestamp
            data.version = Application.version;
            data.timestamp = DateTime.UtcNow.ToString("o");

            // Convert gamedata -> JSON text
            string json = JsonUtility.ToJson(data);

            // Save JSON text to disk
            File.WriteAllText(FilePath, json);

            // Confirm Save
            Debug.Log($"[SaveManager] save game data to {FilePath}");
        }
        catch(Exception e)
        {
            // Confirm No Save
            Debug.Log($"[SaveManager] Save Failed: {e.Message}");
        }
    }

    public static GameData Load()
    {
        TryLoad(out GameData data);
        return data;
    }

    public static bool TryLoad(out GameData data)
    {
        try
        {
            string json = File.ReadAllText(FilePath);
            Debug.Log($"[SaveManager] loaded game data from {FilePath}");
            data = JsonUtility.FromJson<GameData>(json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] Load Failed: {e.Message}");
            data = new GameData();
            return false;
        }
    }
}
