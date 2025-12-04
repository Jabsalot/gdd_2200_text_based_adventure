using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Navigation System/Location")]
public class LocationSO : ScriptableObject
{
    [Header("Identity")]
    public string locationID;
    public string locationName;

    [Header("Dialogue")]
    [Tooltip("The dialogue node that plays when entering this room")]
    public string entryNodeID;

    [Header("Connections")]
    [Tooltip("Rooms the player can travel to from here")]
    public List<LocationConnection> connections = new();

    [Header("Conditional Access")]
    [Tooltip("Flags required to enter this room")]
    public List<string> requiredFlags = new();
    [Tooltip("If any of these flags present, room is inaccessible")]
    public List<string> forbiddenFlags = new();
}

[System.Serializable]
public class LocationConnection
{
    public string displayName;
    public LocationSO targetRoom;
    public List<string> requiredFlags = new();
    public List<string> forbiddenFlags = new();
}
