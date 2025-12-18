using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called every time a scene finishes loading.
    /// Re-acquires all manager references from the new scene.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAllReferences();
    }

    /// <summary>
    /// Finds all required references in the current scene.
    /// </summary>
    private void FindAllReferences()
    {
        player = FindFirstObjectByType<Player>();
        flagManager = FindFirstObjectByType<FlagManager>();
        questManager = FindFirstObjectByType<QuestManager>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (player == null) Debug.LogWarning("[GameManager] Player not found in scene!");
        if (flagManager == null) Debug.LogWarning("[GameManager] FlagManager not found in scene!");
        if (questManager == null) Debug.LogWarning("[GameManager] QuestManager not found in scene!");
        if (dialogueManager == null) Debug.LogWarning("[GameManager] DialogueManager not found in scene!");
    }

    /// <summary>
    /// Checks if all references are valid.
    /// </summary>
    private bool HasValidReferences()
    {
        return player != null && flagManager != null && questManager != null && dialogueManager != null;
    }

    /// <summary>
    /// Saves the current game state to disk.
    /// </summary>
    public void SaveGame()
    {
        if (!HasValidReferences())
        {
            FindAllReferences();
            if (!HasValidReferences())
            {
                Debug.LogError("[GameManager] Cannot save - missing references!");
                return;
            }
        }

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
        // Re-find references in case scene just reloaded
        FindAllReferences();

        if (!HasValidReferences())
        {
            Debug.LogError("[GameManager] Cannot initialize new game - missing references!");
            return;
        }

        currentData = new GameData();
        player.FromData(currentData.playerData);
        flagManager.FromSaveData(currentData.flagData);
        questManager.FromSaveData(currentData.questData);
        dialogueManager.LoadFromNodeID(currentData.currentNodeID);
        Debug.Log($"[GameManager] New Game Initialized!");
    }

    /// <summary>
    /// Initializes the game from loaded save data.
    /// </summary>
    public void InitializeFromSave(GameData loadedData)
    {
        // Re-find references in case scene just reloaded
        FindAllReferences();

        if (!HasValidReferences())
        {
            Debug.LogError("[GameManager] Cannot initialize from save - missing references!");
            return;
        }

        currentData = loadedData;
        player.FromData(currentData.playerData);
        flagManager.FromSaveData(currentData.flagData);
        questManager.FromSaveData(currentData.questData);
        dialogueManager.LoadFromNodeID(currentData.currentNodeID);
        Debug.Log($"[GameManager] Game Initialized from Save!");
    }
}
