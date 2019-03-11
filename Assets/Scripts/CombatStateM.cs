using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateM : MonoBehaviour
{
    // enumeration of combat states. Will have a seperate class associated with each.
    public GameObject PlayerStats;
    public GameObject EnemyStats;
    public enum CombatState
    {
        PlayerPhase,
        PlayerCombat,
        EnemyPhase,
        EnemyCombat,
        Win,
        Defeat
    }
    public CombatState currState;
    // Start is called before the first frame update
    void Start()
    {
        currState = CombatState.PlayerPhase;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (CombatState.PlayerPhase):
                
                break;
            case (CombatState.PlayerCombat):
                //resulting damage and statuses. Message of actions taken. Click to move on'
                //calculateDamage()
                if (EnemyStats.GetComponent<UnitStats>().health <= 0)
                {
                    currState = CombatState.Win;
                }
                else
                {
                    currState = CombatState.EnemyPhase;
                }
                break;
            case (CombatState.EnemyPhase):
                //criminalAI to determine enemy behavior
                
                currState = CombatState.EnemyCombat;
                break;
            case (CombatState.EnemyCombat):
                //resulting damage and statuses. Message of actions taken Click to move on
                //calculateDamage();
                if (PlayerStats.GetComponent<UnitStats>().health <= 0)
                {
                    currState = CombatState.Defeat;
                }
                else
                {
                    currState = CombatState.PlayerPhase;
                }
                break;
            case (CombatState.Win):
                //Win condition

                PlayerStats.GetComponent<UnitStats>().experience = calculateExperience(EnemyStats.GetComponent<UnitStats>(), PlayerStats.GetComponent<UnitStats>().experience);
                PlayerStats.GetComponent<UnitStats>().levelUp();
                break;
            case (CombatState.Defeat):
                //Loss condition
                break;
        }
    }
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
            return (int)Mathf.Ceil(atk / def * mp * 1.5f);
        return (int)Mathf.Ceil(atk / def * mp);
    }

    public int calculateExperience(UnitStats enemyStats, int oldExp)
    {
        return oldExp + enemyStats.health + enemyStats.stamina + enemyStats.attack + enemyStats.defense + enemyStats.speed + enemyStats.luck;
    }
}
