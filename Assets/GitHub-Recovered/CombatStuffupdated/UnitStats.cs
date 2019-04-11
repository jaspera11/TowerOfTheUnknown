using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to whatever unit you want

public class UnitStats : MonoBehaviour
{
    public int level = 1;
    public int health = 200;
    public int maxhealth = 200;
    public int stamina = 50;
    public int maxstamina = 50;
    public int attack = 10;
    public int defense = 10;
    public int speed = 10;
    public int skill = 10;
    public int luck = 10;
    public int experience = 10;
    private int maxExperience = 10;
    private int maxLevel = 10;
    public string entityType;

    public void levelUp()
    {
        if (level == maxLevel) return;
        if (experience >= maxExperience)
        {
            experience -= maxExperience;
            maxExperience *= 2;
            level++;
            maxhealth += 30;
            maxstamina += 15;
            attack += 10;
            defense += 10;
            speed += 10;
            skill += 5;
            luck += 5;
        }
    }
}
