using UnityEngine;

public class PlayerInfoManager : Singleton<PlayerInfoManager>
{
    // Example of player information
    public int playerHealth;
    public int playerGold;
    // Add other player-related properties here

    // Backpack inventory (assuming a simple representation for now)
    public int backpackWidth = 5;
    public int backpackHeight = 5;
    // You can use a more complex structure for the backpack based on your needs

    // Other methods and properties to manage player information and backpack
}
