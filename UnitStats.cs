using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to whatever unit you want

public class UnitStats : MonoBehaviour
{
    public int level;
    public int health;
    public int stamina;
    public int attack;
    public int defense;
    public int speed;
    public int skill;
    public int luck;
    public int experience;
    private int maxExperience;
    private int maxLevel;

    private void Start()
    {
        level = 1;
        health = 200;
        stamina = 50;
        attack = 10;
        defense = 10;
        speed = 10;
        skill = 10;
        luck = 10;
        experience = 0;
        maxExperience = 100;
        maxLevel = 10;
    }

    private void levelUp()
    {
        if (level == maxLevel) return;
        if (experience >= maxExperience)
        {
            experience -= maxExperience;
            maxExperience *= 2;
            level++;
            health += 30;
            stamina += 15;
            attack += 10;
            defense += 10;
            speed += 10;
            skill += 5;
            luck += 5;
        }
    }
}
