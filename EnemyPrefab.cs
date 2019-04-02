using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to parent of enemy units
public class EnemyPrefab : MonoBehaviour
{
    // List of child enemies of this object
    public List<UnitStats> enemies;
    
    void Start()
    {
        // If the enemy is new, add it to the isDeadEnemy list as false (not dead)
        if (!PlayerPrefab.gameData.isDeadEnemy.ContainsKey(gameObject.name))
        {
            PlayerPrefab.gameData.isDeadEnemy.Add(gameObject.name, false);
        }

        // If the enemy is dead, destroy the object, else keep it for next scene
        // WEAKNESS: Does not observe singleton pattern when entering new level and returning to previous
        if (PlayerPrefab.gameData.isDeadEnemy[gameObject.name])
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
