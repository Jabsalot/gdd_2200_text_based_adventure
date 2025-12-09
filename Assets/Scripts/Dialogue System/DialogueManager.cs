using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the flow of dialogue within the game.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Data")]
    public DialogueDatabase database;
    public FlagManager flagManager;
    public string StartNodeID;

    // Events for UI Updates
    public delegate void DialogueUpdated(string speakerName, string dialogueText, List<DialogueChoices> choices);
    public event DialogueUpdated OnDialogueUpdated;

    private DialogueNode currentDialogueNode;
    public DialogueNode GetCurrentDialogueNode => currentDialogueNode;

    private void Start()
    {
        GoToNode(StartNodeID);
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    private void ReloadScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Checks if choice is valid based on the current Flags the player has.
    /// </summary>
    /// <param name="choice"></param>
    /// <returns></returns>
    private bool IsChoiceAvailable(DialogueChoices choice)
    {
        // Check required and forbidden flags
        foreach(var required in choice.requiredFlags)
        {
            if (!flagManager.HasFlag(required)) return false;
        }

        foreach(var forbidden in choice.forbiddenFlags)
        {
            if (flagManager.HasFlag(forbidden)) return false;
        }

        return true;
    }

    /// <summary>
    /// Filter choices the player can currently choose.
    /// </summary>
    /// <param name="choices"></param>
    /// <returns></returns>
    private List<DialogueChoices> FilterChoices(List<DialogueChoices> choices)
    {
        List<DialogueChoices> dialogueChoices = new List<DialogueChoices>();
        foreach(var choice in choices)
        {
            if (IsChoiceAvailable(choice))
                dialogueChoices.Add(choice);
        }
        return dialogueChoices;
    }

    /// <summary>
    /// Select a choice and progress the dialogue.
    /// Buttons in the UI subscribe to this method.
    /// </summary>
    /// <param name="index"></param>
    public void SelectChoice(int index)
    {
        List<DialogueChoices> filtered = FilterChoices(currentDialogueNode.choices);
        DialogueChoices choice = filtered[index];

        foreach (var flag in choice.grantFlags)
        {
            flagManager.AddFlag(flag);
        }

        if (choice.reloadScene)
        {
            ReloadScene();
            return;
        }

        GoToNode(choice.nextNodeID);
    }

    /// <summary>
    /// Go to a specific dialogue node by its ID.
    /// </summary>
    /// <param name="nodeID"></param>
    public void GoToNode(string nodeID)
    {
        currentDialogueNode = database.GetNode(nodeID);

        // If node is null, dialogue has ended. Notify UI.
        if (currentDialogueNode == null)
        {
            OnDialogueUpdated?.Invoke("", "[Dialogue Ended]", null);
            return;
        }

        // Notify UI of updated dialogue state.
        List<DialogueChoices> filtered = FilterChoices(currentDialogueNode.choices);
        {
            OnDialogueUpdated?.Invoke(currentDialogueNode.speakerName, currentDialogueNode.dialogueText, filtered);
        }
    }

    /// <summary>
    /// Gets the current node ID for saving.
    /// </summary>
    public string GetCurrentNodeID()
    {
        return currentDialogueNode?.nodeID ?? StartNodeID;
    }

    /// <summary>
    /// Loads the dialogue state from a node ID.
    /// </summary>
    public void LoadFromNodeID(string nodeID)
    {
        if (!string.IsNullOrEmpty(nodeID))
        {
            GoToNode(nodeID);
        }
        else
        {
            GoToNode(StartNodeID);
        }
    }

}
