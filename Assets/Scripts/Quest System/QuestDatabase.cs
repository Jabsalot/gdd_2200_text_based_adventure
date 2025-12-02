using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Central location for looking up quest information
/// </summary>
[CreateAssetMenu(menuName = "Questing/QuestDatabase")]
public class QuestDatabase : ScriptableObject
{
    public List<QuestSO> allQuests = new();

    private Dictionary<string, QuestSO> lookup; // KEY: Quest Node - VALUE: Quest Scritpable Object

    private void OnEnable()
    {
        lookup = null; // Force rebuild on enable
    }

    private void BuildLookup()
    {
        if (lookup != null) return;

        lookup = new Dictionary<string, QuestSO>();
        foreach (var quest in allQuests)
        {
            if (quest != null && !string.IsNullOrEmpty(quest.questID))
            {
                if (!lookup.ContainsKey(quest.questID))
                {
                    lookup.Add(quest.questID, quest);
                }
                else
                {
                    Debug.LogWarning($"[QuestDatabase] Duplicate quest ID: {quest.questID}");
                }
            }
        }
    }

    public QuestSO GetQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return null;
        BuildLookup();
        lookup.TryGetValue(questID, out QuestSO quest);
        return quest;
    }

    public List<QuestSO> GetAllQuests()
    {
        return allQuests;
    }
}