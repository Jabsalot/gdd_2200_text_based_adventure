using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Controls map navigation and location selection.
/// </summary>
public class MapUI : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dialogueManager;
    public FlagManager flagManager;

    [Header("Locked Message")]
    public GameObject lockedMessagePanel;
    public TextMeshProUGUI lockedMessageText;
    public float messageDisplayTime = 2.5f;

    [Header("Locations")]
    public List<MapLocation> locations = new();

    // Events for when a location is selected
    public delegate void MapOpened();
    public static event MapOpened OnMapOpened;

    public delegate void LocationSelected();
    public static event LocationSelected OnLocationSelected;

    private void Start()
    {
        // Wire up button clicks
        for (int i = 0; i < locations.Count; i++)
        {
            int index = i; // Capture for closure
            locations[i].button.onClick.AddListener(() => OnLocationClicked(index));
        }

        // Hide locked message on start
        if (lockedMessagePanel != null)
        {
            lockedMessagePanel.SetActive(false);
        }

        UpdateButtonStates();
    }

    private void OnEnable()
    {
        if (flagManager != null)
        {
            flagManager.OnFlagAdded += OnFlagChanged;
        }
    }

    private void OnDisable()
    {
        if (flagManager != null)
        {
            flagManager.OnFlagAdded -= OnFlagChanged;
        }
    }

    /// <summary>
    /// Called to open the map UI
    /// References via Button OnClick event
    /// </summary>
    public void OpenMap()
    {
        Debug.Log("[MapTraversal] Map Opened");
        OnMapOpened.Invoke();
    }

    /// <summary>
    /// Called when a flag is added/changed
    /// </summary>
    /// <param name="flag"></param>
    private void OnFlagChanged(string flag)
    {
        UpdateButtonStates();
    }

    /// <summary>
    /// Update visual state of all location buttons based on current flags
    /// </summary>
    private void UpdateButtonStates()
    {
        foreach (var location in locations)
        {
            bool isUnlocked = IsLocationUnlocked(location);
            
            // This lets player click and see why it's locked
            var colors = location.button.colors;
            colors.normalColor = isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
            location.button.colors = colors;
        }
    }

    /// <summary>
    /// Check if a location is unlocked based on flags
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private bool IsLocationUnlocked(MapLocation location)
    {
        if (string.IsNullOrEmpty(location.requiredFlag))
            return true;

        return flagManager.HasFlag(location.requiredFlag);
    }

    /// <summary>
    /// Called when a location button is clicked
    /// </summary>
    /// <param name="index"></param>
    private void OnLocationClicked(int index)
    {
        MapLocation location = locations[index];

        if (!IsLocationUnlocked(location))
        {
            ShowLockedMessage(location.lockedMessage);
            return;
        }

        // Travel to location
        dialogueManager.GoToNode(location.nodeID);
        OnLocationSelected?.Invoke();
    }

    /// <summary>
    /// Show a message indicating the location is locked
    /// </summary>
    /// <param name="message"></param>
    private void ShowLockedMessage(string message)
    {
        if (lockedMessagePanel == null || lockedMessageText == null)
        {
            Debug.Log($"[MapUI] Location locked: {message}");
            return;
        }

        lockedMessageText.text = message;
        lockedMessagePanel.SetActive(true);

        CancelInvoke(nameof(HideLockedMessage));
        Invoke(nameof(HideLockedMessage), messageDisplayTime);
    }

    /// <summary>
    /// Hide the locked message panel
    /// </summary>
    private void HideLockedMessage()
    {
        if (lockedMessagePanel != null)
        {
            lockedMessagePanel.SetActive(false);
        }
    }
}

[System.Serializable]
public class MapLocation
{
    public string locationName;
    public Button button;
    public string nodeID;

    [Header("Lock Settings")]
    public string requiredFlag;
    [TextArea(1, 2)]
    public string lockedMessage = "This path is blocked.";
}
