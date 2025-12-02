using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Questing/QuestData")]
public class QuestSO : ScriptableObject
{
    [Header("Identity")]
    public string questName;

    [Header("Quest Details")]
    public string questGiver;
    [TextArea(2, 5)]
    public string questDetails;

    [Header("Step-By-Step Objectives")]
    public List<string> requiredActivited;

    [Header("Activation Requirement Flags")]
    public List<string> activationFlags;

    [Header("Completion Requirement Flags")]
    public List<string> completationFlags;
}
