using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueNode> Nodes = new();

    private Dictionary<string, DialogueNode> lookup; // KEY: Node ID - VALUE: Node Scriptable Object

    private void BuildLookup()
    {
        if (lookup != null) return;

        lookup = new Dictionary<string, DialogueNode>();

        foreach (DialogueNode node in Nodes)
        {
            lookup.Add(node.nodeID, node);
        }
    }

    public DialogueNode GetNode(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        BuildLookup();
        lookup.TryGetValue(id, out DialogueNode node);
        return node;
    }
}
