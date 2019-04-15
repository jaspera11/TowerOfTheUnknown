using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to PlayerData object (just stores PlayerData)
// Only create in first level
public class PlayerPrefab : MonoBehaviour
{
    public static PlayerPrefab gameData;

    public Dictionary<string, bool> isDeadEnemy;    // List of enemies and whether they're defeated
    public List<PlayerStats> players;               // List of player characters (index 0 is main char)
    public int prevSceneIndex;                      // Previous scene (before battle scene)
    public Transform playerLocation;                // Stores player location in overworld
    public readonly int[] spdTH = { 50, 120 };      // Speed thresholds for chaining attacks
    public readonly float[] sklTH = { 25, 50 };     // Skill thresholds for chaining attacks
    public List<Item> inventory;                    // List of items currently held

    // Prevents duplicate players
    private void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
            isDeadEnemy = new Dictionary<string, bool>();
        }
        else if (gameData != this)
        {
            Destroy(gameObject);
        }
    }
}
