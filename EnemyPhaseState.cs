using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BT;

public class EnemyPhaseState : MonoBehaviour
{
    public GameObject SkillsList;
    public GameObject CombatStateMachine;
    public GameObject PlayerStats;
    public GameObject EnemyStats;
    public Text StatusWindowText;
    public Text HealthText;
    public Text StaminaText;

    // Start is called before the first frame update
    void Start()
    {
        StatusWindowText.text = "The Enemy is Contemplating...";
        HealthText.text = "";
        StaminaText.text = "";
    }

    public bool playerBuffedDefense()
    {
        if(PlayerStats.GetComponent<UnitStats>().defense.defense > 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Attack()
    {
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 5;
        StatusWindowText.text = "The enemy strikes";
    }

    public void debuffDefense()
    {
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 0;
        PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense / 2;
        StatusWindowText.text = "The enemy screeched, lowering your defense";
    }

    public void chooseOption()
    {
        List<Node> level2 = new List<Node>();
        level2.Add(new Call(Attack));
        List<Node> level3 = new List<Node>();
        level3.Add(new Call(debuffDefense));
        level2.Add(new If(playerBuffedDefense,level3));
        Selector Root = new Selector(level2);
        /*Current implementation: First check if player buffed defense.  If yes, debuff defense defense.  If no, attack
        */

        //if (PlayerStats.GetComponent<UnitStats>().defense > 10)
        //{
        //    CombatStateMachine.GetComponent<CombatStateM>().movePower = 0;
        //    PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense / 2;
        //    StatusWindowText.text = "The enemy screeched, lowering your defense";
        //}
        //else
        //{
        //    CombatStateMachine.GetComponent<CombatStateM>().movePower = 5;
        //    StatusWindowText.text = "The enemy strikes";
            
        //}

        if(Root.Evaluate() == Node.NodeStates.SUCCESS)
        {
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.EnemyCombat;
        } 

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
