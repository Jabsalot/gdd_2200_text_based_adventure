using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "rDialogue/DialogueNode")]
public class RDialogueNode : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("The given ID (identity) to the dialogue")]
    private string nodeID;
    public string NodeID => nodeID;
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(nodeID))
        {
            nodeID = name;
        }
    }

    [Header("Dialogue")]
    public string speakerName;
    [TextArea(2, 5)]
    public string dialogueText;

    [Header("Choices")]
    [Tooltip("The choices the user can make to the dialogue")]
    public List<RDialogueChoices> choices = new();
}

[System.Serializable]
public class RDialogueChoices
{
    [Header("UI")]
    public string choiceText;

    [Header("Flow")]
    public RDialogueNode nextNode;
    public bool reloadScene;

    [Header("Conditions")]
    [Tooltip("Flags required for choice to display")]
    public List<DialogueFlag> requiredFlags = new();
    [Tooltip("Flags not allowed for choice to display")]
    public List<DialogueFlag> forbiddenFlags = new();

    [Header("Flags on select")]
    [Tooltip("Flags granted on selection")]
    public List<DialogueFlag> grantFlags = new();
}
