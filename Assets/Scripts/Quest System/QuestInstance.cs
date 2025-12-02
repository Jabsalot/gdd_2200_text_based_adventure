using System;

/// <summary>
/// Runtime instance of a quest tracking current progress.
/// This is what gets saved/loaded.
/// </summary>
[Serializable]
public class QuestInstance
{
    public string questID;
    public QuestStatus status;
    public string currentStageID;

    public QuestInstance(string questID, string initialStageID)
    {
        this.questID = questID;
        this.status = QuestStatus.Active;
        this.currentStageID = initialStageID;
    }
}

public enum QuestStatus
{
    Inactive,   // Not yet available
    Available,  // Can be started
    Active,     // In progress
    Completed,  // Finished successfully
    Failed      // Failed (optional, for future use)
}
