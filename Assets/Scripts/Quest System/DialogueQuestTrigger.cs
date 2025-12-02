using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Bridges dialogue and quests. Add to the same GameObject as DialogueManager.
/// Listens for specific flags and triggers quest actions.
/// </summary>
public class DialogueQuestTrigger : MonoBehaviour
{
    [Header("References")]
    public FlagManager flagManager;
    public QuestManager questManager;

    [Header("Quest Triggers")]
    [Tooltip("Map flag names to quest IDs - when flag is added, quest starts")]
    public List<QuestTriggerMapping> questStartTriggers = new();

    private void OnEnable()
    {
        if (flagManager != null)
        {
            flagManager.OnFlagAdded += OnFlagAdded;
        }
    }

    private void OnDisable()
    {
        if (flagManager != null)
        {
            flagManager.OnFlagAdded -= OnFlagAdded;
        }
    }

    private void OnFlagAdded(string flag)
    {
        foreach (var mapping in questStartTriggers)
        {
            if (mapping.triggerFlag == flag)
            {
                questManager.TryStartQuest(mapping.questID);
            }
        }
    }
}

[System.Serializable]
public class QuestTriggerMapping
{
    [Tooltip("When this flag is added...")]
    public string triggerFlag;
    [Tooltip("...start this quest")]
    public string questID;
}