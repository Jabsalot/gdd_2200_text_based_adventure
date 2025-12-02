using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueNode> Nodes = new();

    private Dictionary<string, DialogueNode> lookup;

    private void BuildNodeDictionary()
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
        BuildNodeDictionary();
        lookup.TryGetValue(id, out DialogueNode node);
        return node;
    }
}
