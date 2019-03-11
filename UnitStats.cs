using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to whatever unit you want

public class UnitStats : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int stamina;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int skill;
    [SerializeField] int luck;
    [SerializeField] int experience = 0;
}
