using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For player units (attach to children objects of player prefab)
public class PlayerStats : UnitStats
{
    public Item currItem;           // Item that player unit is about to use
    public bool itemUsed = false;   // True if item has been used for that turn
    private float experience = 0;   // Current experience for a level
    private float maxExp;           // Max experience for a level required to level up
    private int maxLevel = 10;      // Maximum level (cannot level up after)
    public int skill;               // Determines whether certain skills can be used

    // Modifiable fields, determines stat gain upon leveling up
    [SerializeField] private const float hpUp = 30;
    [SerializeField] private const float spUp = 15;
    [SerializeField] private const float expMod = 2;
    [SerializeField] private const float atkUp = 10;
    [SerializeField] private const float defUp = 10;
    [SerializeField] private const float spdUp = 10;
    [SerializeField] private const int sklUp = 5;
    [SerializeField] private const float luckUp = 5;

    // Adds experience to player unit, levels up there's enough experience
    public void AddExperience(float exp)
    {
        if (level == maxLevel)
        {
            return;
        }
        experience += exp;

        // If there's enough experience, level up the player unit
        while (experience >= maxExp)
        {
            level++;
            experience -= (level == maxLevel) ? experience : maxExp;
            maxExp *= expMod;
            maxHealth += hpUp;
            maxStamina += spUp;
            attack += atkUp;
            defense += defUp;
            speed += spdUp;
            skill += sklUp;
            luck += luckUp;
        }
    }
}

// Parameters for items
[System.Serializable]
public struct Item
{
    public string name;
    public int count;

    public Item(string name, int count)
    {
        this.name = name;
        this.count = count;
    }
}
