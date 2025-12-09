using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages game flags for tracking player choices and states.
/// </summary>
public class FlagManager : MonoBehaviour
{
    private HashSet<string> flags = new();

    // Event fired when a flag is added
    public delegate void FlagEvent(string flag);
    public event FlagEvent OnFlagAdded;
    public event FlagEvent OnFlagRemoved;

    [Header("Debug")]
    public bool debugLog = false;

    // ============ Public Functions ============

    public bool HasFlag(string flag)
    {
        return flags.Contains(flag);
    }

    public void AddFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return;

        if (flags.Add(flag))
        {
            if (debugLog) Debug.Log($"[FlagManager] Flag added: {flag}");
            OnFlagAdded?.Invoke(flag);
        }
    }

    public void RemoveFlag(string flag)
    {
        if (string.IsNullOrEmpty(flag)) return;

        if (flags.Remove(flag))
        {
            if (debugLog) Debug.Log($"[FlagManager] Flag removed: {flag}");
            OnFlagRemoved?.Invoke(flag);
        }
    }

    public List<string> GetAllFlags()
    {
        return new List<string>(flags);
    }

    // ============ Save/Load Support ============

    public FlagSaveData ToSaveData()
    {
        return new FlagSaveData { flags = new List<string>(flags) };
    }

    public void FromSaveData(FlagSaveData data)
    {
        flags.Clear();
        if (data?.flags != null)
        {
            foreach (var flag in data.flags)
            {
                flags.Add(flag);
            }
        }
        if (debugLog) Debug.Log($"[FlagManager] Loaded {flags.Count} flags");
    }
}

[System.Serializable]
public class FlagSaveData
{
    public List<string> flags = new();
}


