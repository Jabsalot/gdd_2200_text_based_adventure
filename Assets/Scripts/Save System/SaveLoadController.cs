using UnityEngine;

/// <summary>
/// Controls the Save and Load functionality from the UI.
/// These functions should be hooked to buttons where saving, loading, or starting a new game is needed.
/// </summary>
public class SaveLoadController : MonoBehaviour
{
    // Events for when game starts
    public delegate void GameStarted();
    public static event GameStarted OnGameStarted;

    public void OnNewGameClicked()
    {
        Debug.Log("[UIMainMenu] New Game Clicked");
        GameManager.Instance.InitializeNewGame();

        // Notify listeners that game has started (e.g., UIManager to show main UI)
        OnGameStarted?.Invoke();
    }

    public void OnLoadGameClicked()
    {
        Debug.Log("[UIMainMenu] Load Game Clicked");
        GameData loadedData = SaveManager.Load();
        GameManager.Instance.InitializeFromSave(loadedData);

        // Notify listeners that game has started (e.g., UIManager to show main UI)
        OnGameStarted?.Invoke();
    }

    public void OnSaveGameClicked()
    {
        Debug.Log("[UIMainMenu] Save Game Clicked");
        GameManager.Instance.SaveGame();
    }
}
