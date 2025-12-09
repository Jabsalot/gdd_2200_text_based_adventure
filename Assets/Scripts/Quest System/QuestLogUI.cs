using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestLogUI : MonoBehaviour
{
    [Header("References")]
    public QuestManager questManager;
    public QuestDatabase questDatabase;

    [Header("UI Elements")]
    public GameObject questLogPanel;
    public Transform questListContainer;
    public GameObject questEntryPrefab;
    public TextMeshProUGUI detailsTitle;
    public TextMeshProUGUI detailsDescription;
    public TextMeshProUGUI detailsObjective;

    [Header("Toggle Key")]
    public KeyCode toggleKey = KeyCode.J;

    private List<GameObject> spawnedEntries = new();
    private string selectedQuestID;

    // Events for when quest log is selected or clicked
    public delegate void QuestLogActivated();
    public static event QuestLogActivated OnQuestLogTriggered;

    private void OnEnable()
    {
        if (questManager != null)
        {
            questManager.OnQuestStarted += OnQuestChanged;
            questManager.OnQuestCompleted += OnQuestChanged;
            questManager.OnObjectiveUpdated += OnObjectiveChanged;
        }
    }

    private void OnDisable()
    {
        if (questManager != null)
        {
            questManager.OnQuestStarted -= OnQuestChanged;
            questManager.OnQuestCompleted -= OnQuestChanged;
            questManager.OnObjectiveUpdated -= OnObjectiveChanged;
        }
    }

    /// <summary>
    /// Toggles the QuestLogUI.
    /// Function should be used by a buttons OnClick() event.
    /// </summary>
    public void ToggleQuestLogUI()
    {
        Debug.Log("[QuestLogUI] Quest Log Opened");

        // Refresh Quest Log UI in case any quests require update since last viewing
        //RefreshQuestList();

        // Notify listeners that quest journal was triggered
        OnQuestLogTriggered?.Invoke();
        
    }

    private void OnQuestChanged(string questID, QuestSO quest)
    {
        if (questLogPanel.activeSelf)
        {
            RefreshQuestList();
        }
    }

    private void OnObjectiveChanged(string questID, QuestStage stage)
    {
        if (questLogPanel.activeSelf && selectedQuestID == questID)
        {
            ShowQuestDetails(questID);
        }
    }

    public void RefreshQuestList()
    {
        // Clear existing entries
        foreach (var entry in spawnedEntries)
        {
            Destroy(entry);
        }
        spawnedEntries.Clear();

        // Add active quests
        List<QuestSO> activeQuests = questManager.GetActiveQuests();
        foreach (var quest in activeQuests)
        {
            CreateQuestEntry(quest, false);
        }

        // Optionally show completed quests (grayed out)
        foreach (var quest in questDatabase.GetAllQuests())
        {
            if (questManager.IsQuestCompleted(quest.questID))
            {
                CreateQuestEntry(quest, true);
            }
        }

        // Select first quest if none selected
        if (string.IsNullOrEmpty(selectedQuestID) && activeQuests.Count > 0)
        {
            SelectQuest(activeQuests[0].questID);
        }
        else if (!string.IsNullOrEmpty(selectedQuestID))
        {
            ShowQuestDetails(selectedQuestID);
        }
    }

    private void CreateQuestEntry(QuestSO quest, bool isCompleted)
    {
        GameObject entry = Instantiate(questEntryPrefab, questListContainer);
        spawnedEntries.Add(entry);

        TextMeshProUGUI label = entry.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
        {
            string prefix = isCompleted ? "[DONE] " : "";
            label.text = prefix + quest.questName;

            if (isCompleted)
            {
                label.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }

        Button button = entry.GetComponent<Button>();
        if (button != null)
        {
            string questID = quest.questID;
            button.onClick.AddListener(() => SelectQuest(questID));
        }
    }

    public void SelectQuest(string questID)
    {
        selectedQuestID = questID;
        ShowQuestDetails(questID);
    }

    private void ShowQuestDetails(string questID)
    {
        QuestSO quest = questDatabase.GetQuest(questID);
        if (quest == null)
        {
            ClearDetails();
            return;
        }

        detailsTitle.text = quest.questName;
        detailsDescription.text = quest.questDescription;

        if (questManager.IsQuestCompleted(questID))
        {
            detailsObjective.text = "<i>Quest Completed</i>";
        }
        else
        {
            QuestStage currentStage = questManager.GetCurrentStage(questID);
            if (currentStage != null)
            {
                detailsObjective.text = $"<b>Objective:</b> {currentStage.objectiveText}";
            }
            else
            {
                detailsObjective.text = "";
            }
        }
    }

    private void ClearDetails()
    {
        detailsTitle.text = "";
        detailsDescription.text = "Select a quest to view details.";
        detailsObjective.text = "";
    }
}