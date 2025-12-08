using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Location Data")]
    [Tooltip("A list of dialogue nodes for each location on the map. Each index corresponds to a location.")]
    public List<DialogueNode> locationNodes;

    public DialogueManager dialogueManager;

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

        // Close map
        this.gameObject.SetActive(false);
    }

}
