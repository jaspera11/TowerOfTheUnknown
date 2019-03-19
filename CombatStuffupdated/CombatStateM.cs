using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatStateM : MonoBehaviour
{
    // enumeration of combat states. Will have a seperate class associated with each.
    public GameObject PlayerStats;
    public GameObject EnemyStats;
    public Text StatusWindowText;
    public Text Player1HP;
    public Text Player1SP;
    public Text Enemy1HP;
    public Text Enemy1SP;
    public int movePower;
    public int battleDmg;
    public string Enemy1type;
    public string Enemy2type;
    public string Enemy3type;
    public enum CombatState
    {
        PlayerPhase,
        //Player2Phase,
        //Player3Phase,
        PlayerCombat,
        EnemyPhase,
        Enemy2Phase,
        Enemy3Phase,
        EnemyCombat,
        Win,
        Defeat
    }
    public CombatState currState;
    // Start is called before the first frame update
    void Start()
    {
        currState = CombatState.PlayerPhase;
        Enemy1type = EnemyStats.GetComponent<UnitStats>().entityType;
        PlayerStats.GetComponent<UnitStats>().stamina = PlayerStats.GetComponent<UnitStats>().maxstamina;
        EnemyStats.GetComponent<UnitStats>().health = EnemyStats.GetComponent<UnitStats>().maxhealth;
        EnemyStats.GetComponent<UnitStats>().stamina = EnemyStats.GetComponent<UnitStats>().maxstamina;
        Player1HP.text = "HP: " + PlayerStats.GetComponent<UnitStats>().health  + "/" + PlayerStats.GetComponent<UnitStats>().maxhealth ;
        Player1SP.text = "SP: " + PlayerStats.GetComponent<UnitStats>().stamina + "/" + PlayerStats.GetComponent<UnitStats>().maxstamina;
        Enemy1HP.text = "HP: " + EnemyStats.GetComponent<UnitStats>().health + "/" + EnemyStats.GetComponent<UnitStats>().maxhealth;
        Enemy1SP.text = "SP: " + EnemyStats.GetComponent<UnitStats>().health + "/" + EnemyStats.GetComponent<UnitStats>().maxhealth; 
    }

    // Update is called once per frame
    void Update()
    {
        Player1SP.text = "SP: " + PlayerStats.GetComponent<UnitStats>().stamina + "/" + PlayerStats.GetComponent<UnitStats>().maxstamina;
        Enemy1SP.text = "SP: " + EnemyStats.GetComponent<UnitStats>().stamina + "/" + EnemyStats.GetComponent<UnitStats>().maxstamina;
        Player1HP.text = "HP: " + PlayerStats.GetComponent<UnitStats>().health + "/" + PlayerStats.GetComponent<UnitStats>().maxhealth;
        Enemy1HP.text = "HP: " + EnemyStats.GetComponent<UnitStats>().health + "/" + EnemyStats.GetComponent<UnitStats>().maxhealth;
        switch (currState)
        {
            case (CombatState.PlayerPhase):
                
                break;
            case (CombatState.PlayerCombat):
                //resulting damage and statuses. Message of actions taken. Click to move on'
                //calculateDamage()
                battleDmg = calculateDamage(PlayerStats.GetComponent<UnitStats>().attack, EnemyStats.GetComponent<UnitStats>().defense, movePower, PlayerStats.GetComponent<UnitStats>().luck);
                if (battleDmg >= EnemyStats.GetComponent<UnitStats>().health)
                {
                    EnemyStats.GetComponent<UnitStats>().health = 0;
                    currState = CombatState.Win;
                }
                else
                {
                    EnemyStats.GetComponent<UnitStats>().health = EnemyStats.GetComponent<UnitStats>().health - battleDmg;
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
                //Temporary code throwout later
                movePower = 10;
                battleDmg = calculateDamage(EnemyStats.GetComponent<UnitStats>().attack, PlayerStats.GetComponent<UnitStats>().defense, movePower, EnemyStats.GetComponent<UnitStats>().luck);
                if (PlayerStats.GetComponent<UnitStats>().defense > 10)
                {
                    PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense / 2;
                    movePower = 0;
                }
                if (battleDmg >= PlayerStats.GetComponent<UnitStats>().health)
                {
                    PlayerStats.GetComponent<UnitStats>().health = 0;
                    currState = CombatState.Defeat;

                    PlayerStats.GetComponent<UnitStats>().health = PlayerStats.GetComponent<UnitStats>().health - battleDmg;
                }
                else
                {
                    PlayerStats.GetComponent<UnitStats>().health = PlayerStats.GetComponent<UnitStats>().health - battleDmg;
                    currState = CombatState.PlayerPhase;
                }
                break;
            case (CombatState.Win):
                //Win condition
                PlayerStats.GetComponent<UnitStats>().experience = calculateExperience(EnemyStats.GetComponent<UnitStats>(), PlayerStats.GetComponent<UnitStats>().experience);
                PlayerStats.GetComponent<UnitStats>().levelUp();
                SceneManager.LoadScene(0);
                break;
            case (CombatState.Defeat):
                //Loss condition
                SceneManager.LoadScene(0);
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
        {
            return (int)Mathf.Ceil(atk / def * mp * 1.5f);
            StatusWindowText.text += "\n It was a critical hit!";
        }
        else return (int)Mathf.Ceil(atk / def * mp);
    }

    public int calculateExperience(UnitStats enemyStats, int oldExp)
    {
        return oldExp + enemyStats.health + enemyStats.stamina + enemyStats.attack + enemyStats.defense + enemyStats.speed + enemyStats.luck;
    }
}
