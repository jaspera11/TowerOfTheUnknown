using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineFunc : MonoBehaviour
{
    // copy and paste these into the state machine script ***************

    // returns the percent chance that a crit will happen
    private float calculateCrit(int luck)
    {
        return (Mathf.Log(luck + 100, 1.05f) + 1 - Mathf.Log(100, 1.05f)) / 100f;
    }

    public int calculateDamage(int attack, int defense, int movePower, int luck)
    {
        float atk = attack;
        float def = defense;
        float mp = movePower;
        bool isCrit = Random.Range(0, 1) < calculateCrit(luck);
        if (isCrit)
            return (int) Mathf.Ceil(atk / def * mp * 1.5f);
        return (int) Mathf.Ceil(atk / def * mp);
    }

    public int calculateExperience(UnitStats enemyStats, int oldExp)
    {
        return oldExp + enemyStats.health + enemyStats.stamina + enemyStats.attack + enemyStats.defense + enemyStats.speed + enemyStats.luck;
    }
}
