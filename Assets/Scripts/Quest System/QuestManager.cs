using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the quest system, including starting, tracking, and completing quests.
/// </summary>
public class QuestManager : MonoBehaviour
{
    [Header("Data")]
    public QuestDatabase questDatabase;
    public FlagManager flagManager;

    [Header("Debug")]
    public bool debugLog = true;

    // Runtime quest tracking
    private Dictionary<string, QuestInstance> activeQuests = new();
    private HashSet<string> completedQuestIDs = new();

    // Events for UI updates
    public delegate void QuestEvent(string questID, QuestSO questData);
    public delegate void QuestStageEvent(string questID, QuestStage stage);

    public event QuestEvent OnQuestStarted;
    public event QuestEvent OnQuestCompleted;
    public event QuestStageEvent OnStageCompleted;
    public event QuestStageEvent OnObjectiveUpdated;

    private void OnEnable()
    {
        // Subscribe to flag changes to check quest progress
        if(flagManager != null)
        {
            flagManager.OnFlagAdded += OnFlagChanged;
        }

        // Subscribe to dialogue quest triggers
        DialogueManager.OnQuestTriggered += OnDialogueQuestTriggered;
    }

    private void OnDisable()
    {
        if (flagManager != null)
        {
            flagManager.OnFlagAdded -= OnFlagChanged;
        }

        DialogueManager.OnQuestTriggered -= OnDialogueQuestTriggered;
    }

    private void Start()
    {
        // Subscribe to flag changes to check quest progress
        if (flagManager != null)
        {
            flagManager.OnFlagAdded += OnFlagChanged;
        }
    }

    private void OnDestroy()
    {
        if (flagManager != null)
        {
            flagManager.OnFlagAdded -= OnFlagChanged;
        }
    }

    /// <summary>
    /// Called whenever a flag is added - checks if any quest stages complete
    /// </summary>
    private void OnFlagChanged(string flag)
    {
        CheckAllQuestProgress();
    }

    private void OnDialogueQuestTriggered(string questID)
    {
        TryStartQuest(questID);
    }

    /// <summary>
    /// Check if a quest can be started based on current flags
    /// </summary>
    public bool CanStartQuest(string questID)
    {
        QuestSO quest = questDatabase.GetQuest(questID);
        if (quest == null) return false;

        // Already active or completed?
        if (activeQuests.ContainsKey(questID) || completedQuestIDs.Contains(questID))
            return false;

        // Check required flags
        foreach (var required in quest.activationRequiredFlags)
        {
            if (!flagManager.HasFlag(required)) return false;
        }

        // Check forbidden flags
        foreach (var forbidden in quest.activationForbiddenFlags)
        {
            if (flagManager.HasFlag(forbidden)) return false;
        }

        return true;
    }

    /// <summary>
    /// Start a quest if possible
    /// </summary>
    public bool TryStartQuest(string questID)
    {
        if (!CanStartQuest(questID))
        {
            if (debugLog) Debug.Log($"[QuestManager] Cannot start quest: {questID}");
            return false;
        }

        QuestSO quest = questDatabase.GetQuest(questID);
        QuestStage firstStage = quest.GetFirstStage();

        if (firstStage == null)
        {
            Debug.LogWarning($"[QuestManager] Quest {questID} has no stages!");
            return false;
        }

        // Create runtime instance
        QuestInstance instance = new QuestInstance(questID, firstStage.stageID);
        activeQuests.Add(questID, instance);

        // Grant quest started flag
        if (!string.IsNullOrEmpty(quest.questStartedFlag))
        {
            flagManager.AddFlag(quest.questStartedFlag);
        }

        if (debugLog) Debug.Log($"[QuestManager] Started quest: {quest.questName}");

        OnQuestStarted?.Invoke(questID, quest);
        OnObjectiveUpdated?.Invoke(questID, firstStage);

        // Check if first stage is already complete
        CheckQuestProgress(questID);

        return true;
    }

    /// <summary>
    /// Check progress on all active quests
    /// </summary>
    public void CheckAllQuestProgress()
    {
        // Copy keys to avoid modification during iteration
        List<string> questIDs = new List<string>(activeQuests.Keys);

        foreach (var questID in questIDs)
        {
            CheckQuestProgress(questID);
        }
    }

    /// <summary>
    /// Check if a specific quest's current stage is complete
    /// </summary>
    private void CheckQuestProgress(string questID)
    {
        if (!activeQuests.TryGetValue(questID, out QuestInstance instance))
            return;

        QuestSO quest = questDatabase.GetQuest(questID);
        if (quest == null) return;

        QuestStage currentStage = quest.GetStage(instance.currentStageID);
        if (currentStage == null) return;

        // Check if all required flags are present
        bool stageComplete = true;
        foreach (var required in currentStage.requiredFlags)
        {
            if (!flagManager.HasFlag(required))
            {
                stageComplete = false;
                break;
            }
        }

        if (stageComplete)
        {
            CompleteStage(questID, currentStage);
        }
    }

    /// <summary>
    /// Complete the current stage and advance to next
    /// </summary>
    private void CompleteStage(string questID, QuestStage stage)
    {
        if (!activeQuests.TryGetValue(questID, out QuestInstance instance))
            return;

        QuestSO quest = questDatabase.GetQuest(questID);

        // Grant stage completion flags
        foreach (var flag in stage.grantFlagsOnComplete)
        {
            flagManager.AddFlag(flag);
        }

        if (debugLog) Debug.Log($"[QuestManager] Completed stage: {stage.stageID} in quest {quest.questName}");
        OnStageCompleted?.Invoke(questID, stage);

        // Check for next stage
        if (string.IsNullOrEmpty(stage.nextStageID))
        {
            // Quest complete!
            CompleteQuest(questID);
        }
        else
        {
            // Move to next stage
            QuestStage nextStage = quest.GetStage(stage.nextStageID);
            if (nextStage != null)
            {
                instance.currentStageID = nextStage.stageID;
                if (debugLog) Debug.Log($"[QuestManager] Advanced to stage: {nextStage.stageID}");
                OnObjectiveUpdated?.Invoke(questID, nextStage);

                // Immediately check if next stage is already complete
                CheckQuestProgress(questID);
            }
            else
            {
                Debug.LogWarning($"[QuestManager] Next stage not found: {stage.nextStageID}");
                CompleteQuest(questID);
            }
        }
    }

    /// <summary>
    /// Mark a quest as completed
    /// </summary>
    private void CompleteQuest(string questID)
    {
        if (!activeQuests.TryGetValue(questID, out QuestInstance instance))
            return;

        QuestSO quest = questDatabase.GetQuest(questID);

        instance.status = QuestStatus.Completed;
        activeQuests.Remove(questID);
        completedQuestIDs.Add(questID);

        // Grant completion flag
        if (!string.IsNullOrEmpty(quest.questCompletedFlag))
        {
            flagManager.AddFlag(quest.questCompletedFlag);
        }

        // Grant reward flags
        foreach (var flag in quest.completionRewardFlags)
        {
            flagManager.AddFlag(flag);
        }

        if (debugLog) Debug.Log($"[QuestManager] Completed quest: {quest.questName}");
        OnQuestCompleted?.Invoke(questID, quest);
    }

    // ============ Query Methods ============

    public bool IsQuestActive(string questID)
    {
        return activeQuests.ContainsKey(questID);
    }

    public bool IsQuestCompleted(string questID)
    {
        return completedQuestIDs.Contains(questID);
    }

    public QuestInstance GetQuestInstance(string questID)
    {
        activeQuests.TryGetValue(questID, out QuestInstance instance);
        return instance;
    }

    public QuestStage GetCurrentStage(string questID)
    {
        if (!activeQuests.TryGetValue(questID, out QuestInstance instance))
            return null;

        QuestSO quest = questDatabase.GetQuest(questID);
        return quest?.GetStage(instance.currentStageID);
    }

    public List<QuestSO> GetActiveQuests()
    {
        List<QuestSO> result = new();
        foreach (var questID in activeQuests.Keys)
        {
            QuestSO quest = questDatabase.GetQuest(questID);
            if (quest != null) result.Add(quest);
        }
        return result;
    }

    // ============ Save/Load Support ============

    public QuestSaveData ToSaveData()
    {
        QuestSaveData data = new QuestSaveData();

        foreach (var kvp in activeQuests)
        {
            data.activeQuests.Add(kvp.Value);
        }

        data.completedQuestIDs = new List<string>(completedQuestIDs);

        return data;
    }

    public void FromSaveData(QuestSaveData data)
    {
        activeQuests.Clear();
        completedQuestIDs.Clear();

        if (data == null) return;

        foreach (var instance in data.activeQuests)
        {
            activeQuests[instance.questID] = instance;
        }

        foreach (var questID in data.completedQuestIDs)
        {
            completedQuestIDs.Add(questID);
        }

        if (debugLog) Debug.Log($"[QuestManager] Loaded {activeQuests.Count} active quests, {completedQuestIDs.Count} completed");
    }
}

/// <summary>
/// Serializable data for saving quest state
/// </summary>
[System.Serializable]
public class QuestSaveData
{
    public List<QuestInstance> activeQuests = new();
    public List<string> completedQuestIDs = new();
}