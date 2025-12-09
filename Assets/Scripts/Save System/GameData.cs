[System.Serializable]
public class GameData
{
    public string version   = "v1.1";   // Version of the game data
    public string timestamp = "";       // Last time saved (date, time, etc...)

    public PlayerData playerData = new();   // Reference to player data
    public FlagSaveData flagData = new();   // Reference to flag data
    public QuestSaveData questData = new(); // Reference to quest data
    public string currentNodeID = "";       // Current dialogue node ID
}
