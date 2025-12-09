using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// A single node in a dialogue tree.
/// </summary>
[CreateAssetMenu(menuName="Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [Header("Identity")]
    public string nodeID;

    [Header("Visuals")]
    public Sprite nodeImage;

    [Header("Dialogue")]
    public string speakerName;
    [TextArea(2, 5)]
    public string dialogueText;

    [Header("Choices")]
    public List<DialogueChoices> choices = new();
}

[Serializable]
public class DialogueChoices
{
    [Header("UI")]
    public string choiceText;

    [Header("Flow")]
    public string nextNodeID;
    public bool reloadScene;

    [Header("Conditions")]
    public List<string> requiredFlags = new();
    public List<string> forbiddenFlags = new();

    [Header("Flags on select")]
    public List<string> grantFlags = new();
}

