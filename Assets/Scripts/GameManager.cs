using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    private GameData currentData = new();

    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        currentData.playerData = player.ToData();
        SaveManager.Save(currentData);
        Debug.Log($"[GameManager] Game Saved!");
    }

    public void LoadGame()
    {
        if(SaveManager.TryLoad(out currentData))
        {
            player.FromData(currentData.playerData);
            Debug.Log($"[GameManager] Game Loaded!");
        }
        else
        {
            Debug.LogWarning($"[GameManager] No save file found. Starting new save file.");
            SaveGame();
        }
    }
}
