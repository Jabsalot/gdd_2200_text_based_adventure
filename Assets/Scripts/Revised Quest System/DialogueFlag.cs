using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "rDialogue/DialogueFlag")]
public class DialogueFlag : ScriptableObject
{
    [SerializeField, Tooltip("Auto-generated from asset name")]
    private string flagID;

    public string FlagID => flagID;

    [TextArea(1, 3)]
    public string description;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(flagID))
        {
            flagID = name;
        }
    }
}
