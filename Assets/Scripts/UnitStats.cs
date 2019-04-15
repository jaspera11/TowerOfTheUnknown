using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For enemy units (attach to children objects of enemy prefab)
public class UnitStats : MonoBehaviour
{
    public string charName;         // Character's name (unique)
    public int level = 1;           // Current level
    public List<Skill> skillList;   // List of possible skills
    public float health;            // Current health
    public float maxHealth;         // Maximum health
    public float stamina;           // Current stamina
    public float maxStamina;        // Maximum stamina
    public float attack;            // Attack stat (affects damage)
    public float defense;           // Defense stat (affects damage)
    public float speed;             // Speed stat (affects number of skills in currSkills)
    public float luck;              // Luck stat (affects critical hit chance)
    public string currOption;       // Current option (Attack, Skills, Item, Flee)
    public Stack<Skill> currSkills; // Current skill(s) for that turn (multiple can be chained together)
    public UnitStats currEnemy;     // Current enemy that this unit is targeting

    // Generic function that destroys all objects by a certain tag
    public static void DestroyByTag(string tag)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            Destroy(obj);
        }
    }
}

// Parameters for skills
[System.Serializable]
public struct Skill
{
    public string name;
    public string type;
    public int power; // optional for non-damaging moves
    public int SPCost;
    public int skillReq;

    public Skill(string name, string type, int power, int SPCost, int skillReq)
    {
        this.name = name;
        this.type = type;
        this.power = power;
        this.SPCost = SPCost;
        this.skillReq = skillReq;
    }
}