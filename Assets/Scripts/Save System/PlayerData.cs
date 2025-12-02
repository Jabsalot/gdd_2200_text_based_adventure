using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public float health;

    public Vector3 playerPosition = new Vector3(0, 2, 0);

    public PlayerData()
    {

    }

    public PlayerData(string name, int level, float health, Vector3 playerPosition)
    {
        this.name = name;
        this.level = level;
        this.health = health;
        this.playerPosition = playerPosition;
    }
}
