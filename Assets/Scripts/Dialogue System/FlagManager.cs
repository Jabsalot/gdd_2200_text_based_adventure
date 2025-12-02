using UnityEngine;
using System.Collections.Generic;

public class FlagManager : MonoBehaviour
{
    private HashSet<string> flags = new();

    public bool HasFlag(string flag)
    {
        return flags.Contains(flag);
    }

    public void AddFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return;
        flags.Add(flag);
    }
}


