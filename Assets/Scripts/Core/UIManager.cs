using UnityEngine;

/// <summary>
/// Controls the UI elements visibility and state.
/// Any and all UI panel visibility should be managed through this script.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Save/Load UI")]
    public GameObject SaveLoadUIPanel;
    [Header("Main Game UI")]
    public GameObject DialogueUI;
    public GameObject UtilUI;
    [Header("Map UI")]
    public GameObject MapUIPanel;
    [Header("Quest Log")]
    public GameObject QuestLogPanel;

    private void OnEnable()
    {
        UIUniversalController.onBackButtonPressed += ShowMainUI;

        MapUI.OnMapOpened += ShowMapUI;
        MapUI.OnLocationSelected += ShowMainUI;

        SaveLoadUI.OnGameStarted += ShowMainUI;

        QuestLogUI.OnQuestLogTriggered += ShowQuestLogUI;
    }

    private void OnDisable()
    {
        UIUniversalController.onBackButtonPressed -= ShowMainUI;

        MapUI.OnMapOpened -= ShowMapUI;
        MapUI.OnLocationSelected -= ShowMainUI;

        SaveLoadUI.OnGameStarted -= ShowMainUI;
        
        QuestLogUI.OnQuestLogTriggered += ShowQuestLogUI;
    }

    private void Start()
    {
        ShowSaveLoadUI();
    }

    /// <summary>
    /// Shows the Save/Load UI panel and hides others.
    /// </summary>
    private void ShowSaveLoadUI()
    {
        DialogueUI.SetActive(false);
        UtilUI.SetActive(false);
        MapUIPanel.SetActive(false);
        QuestLogPanel.SetActive(false);

        SaveLoadUIPanel.SetActive(true);
    }

    /// <summary>
    /// Shows the main game UI and hides others.
    /// </summary>
    private void ShowMainUI()
    {
        SaveLoadUIPanel.SetActive(false);
        MapUIPanel.SetActive(false);
        QuestLogPanel.SetActive(false);

        DialogueUI.SetActive(true);
        UtilUI.SetActive(true);
    }

    // TODO: Map should not be toggeled if in dialogue with an NPC
    // TODO: Should only be toggelable if player is in an environment dialogue
    /// <summary>
    /// Shows the Map UI and hides others.
    /// </summary>
    private void ShowMapUI()
    {
        SaveLoadUIPanel.SetActive(false);
        DialogueUI.SetActive(false);
        UtilUI.SetActive(false);
        QuestLogPanel.SetActive(false);

        MapUIPanel.SetActive(true);
    }

    /// <summary>
    /// Shows the QuestLogUI
    /// </summary>
    private void ShowQuestLogUI()
    {
        SaveLoadUIPanel.SetActive(false);
        DialogueUI.SetActive(false);
        UtilUI.SetActive(false);
        MapUIPanel.SetActive(false);

        QuestLogPanel.SetActive(true);
    }
}
