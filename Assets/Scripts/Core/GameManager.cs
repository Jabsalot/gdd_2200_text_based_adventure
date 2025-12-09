using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player player;
    public FlagManager flagManager;
    public QuestManager questManager;
    public DialogueManager dialogueManager;
    public GameData currentData = new();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Saves the current game state to disk.
    /// </summary>
    public void SaveGame()
    {
        currentData.playerData = player.ToData();
        currentData.flagData = flagManager.ToSaveData();
        currentData.questData = questManager.ToSaveData();
        currentData.currentNodeID = dialogueManager.GetCurrentNodeID();
        SaveManager.Save(currentData);
        Debug.Log($"[GameManager] Game Saved!");
    }

    /// <summary>
    /// Initializes a new game with default data.
    /// </summary>
    public void InitializeNewGame()
    {
        currentData = new GameData();
        player.FromData(currentData.playerData);
        flagManager.FromSaveData(currentData.flagData);
        questManager.FromSaveData(currentData.questData);
        dialogueManager.LoadFromNodeID(currentData.currentNodeID); // Will use StartNodeID if empty
        Debug.Log($"[GameManager] New Game Initialized!");
    }

    /// <summary>
    /// Initializes the game from loaded save data.
    /// </summary>
    public void InitializeFromSave(GameData loadedData)
    {
        currentData = loadedData;
        player.FromData(currentData.playerData);
        flagManager.FromSaveData(currentData.flagData);
        questManager.FromSaveData(currentData.questData);
        dialogueManager.LoadFromNodeID(currentData.currentNodeID); // Loads saved node
        Debug.Log($"[GameManager] Game Initialized from Save!");
    }
}
