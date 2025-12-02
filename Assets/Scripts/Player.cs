using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName = "Player";
    public int playerLevel = 1;
    public float playerHealth = 10.0f;

    /// <summary>
    /// Generate new Data from save
    /// </summary>
    /// <returns></returns>
    public PlayerData ToData()
    {
        return new PlayerData(playerName, playerLevel, playerHealth, transform.position);
    }

    /// <summary>
    /// Read data from file to player from load
    /// </summary>
    public void FromData(PlayerData data)
    {
        playerName = data.name; 
        playerLevel = data.level;
        playerHealth = data.health;
        transform.position = data.playerPosition;
    }
}
