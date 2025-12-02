using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// QuestSO in a Quest Definition. It defines everything for a given quest.
/// </summary>
[CreateAssetMenu(menuName = "Questing/Quest")]
public class QuestSO : ScriptableObject
{
    [Header("Identity")]
    public string questID;
    public string questName;

    [Header("Quest Info")]
    public string questGiver;
    [TextArea(2, 5)]
    public string questDescription;

    [Header("Activation")]
    [Tooltip("Flags required for this quest to become available")]
    public List<string> activationRequiredFlags = new();
    [Tooltip("If any of these flags are present, quest cannot be activated")]
    public List<string> activationForbiddenFlags = new();
    [Tooltip("Flag granted when quest is started")]
    public string questStartedFlag;

    [Header("Stages")]
    public List<QuestStage> stages = new();

    [Header("Completion")]
    [Tooltip("Flag granted when entire quest is completed")]
    public string questCompletedFlag;
    [Tooltip("Flags granted as rewards upon completion")]
    public List<string> completionRewardFlags = new();

    /// <summary>
    /// Gets the first stage of the quest
    /// </summary>
    public QuestStage GetFirstStage()
    {
        return stages.Count > 0 ? stages[0] : null;
    }

    /// <summary>
    /// Finds a stage by its ID
    /// </summary>
    public QuestStage GetStage(string stageID)
    {
        foreach (var stage in stages)
        {
            if (stage.stageID == stageID)
                return stage;
        }
        return null;
    }
}

/// <summary>
/// This is an individual stage within a given Quest. 
/// A stage is an action that must be done to further a quest.
/// Quest can have one stage (such as fetch quests) or multiple stages
/// such as meet X person, go to X location, find x item, hand in item.
/// </summary>
[Serializable]
public class QuestStage
{
    [Header("Stage Identity")]
    public string stageID;
    [TextArea(1, 3)]
    public string objectiveText;

    [Header("Completion Conditions")]
    [Tooltip("All of these flags much be present to complete this stage")]
    public List<string> requiredFlags = new();

    [Header("On Stage Complete")]
    [Tooltip("Flags granted when this stage is completed")]
    public List<string> grantFlagsOnComplete = new();

    [Tooltip("ID of the next stage, leave empty if this is the final stage")]
    public string nextStageID;
}

