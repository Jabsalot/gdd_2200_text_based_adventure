using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls map navigation and location selection.
/// </summary>
public class MapUI : MonoBehaviour
{
    [Header("Location Data")]
    [Tooltip("A list of dialogue nodes for each location on the map. Each index corresponds to a location.")]
    public List<DialogueNode> locationNodes;

    public DialogueManager dialogueManager;

    // Events for when a location is selected
    public delegate void MapOpened();
    public static event MapOpened OnMapOpened;

    public delegate void LocationSelected();
    public static event LocationSelected OnLocationSelected;

    public void OpenMap()
    {
        Debug.Log("[MapTraversal] Map Opened");
        OnMapOpened.Invoke();
    }

    public void SelectLocation(int index)
    {
        // Validate index check
        if(index < 0 || index >= locationNodes.Count)
        {
            Debug.LogError($"[MapTraversal] Invalid location index: {index}");
            return;
        }

        // Check that we can enter location based on user flags


        // Get node and go to it
        DialogueNode selectedNode = locationNodes[index];
        dialogueManager.GoToNode(selectedNode.nodeID);

        // Notify listeners that a location was selected
        OnLocationSelected?.Invoke();
    }

}
