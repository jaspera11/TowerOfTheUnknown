using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    public GameObject skillsMenu;
    public GameObject stateMachine;
    private TBCombatStateM.CombatState currState;
    private TBCombatStateM.CombatState prevState;
    private int attack;
    private bool usedHealthPack;
    public void OnAttackPressed()
    {
        if (currState == TBCombatStateM.CombatState.PlayerSelect)
        {
            prevState = TBCombatStateM.CombatState.PlayerSelect;
            currState = TBCombatStateM.CombatState.Combat;
            attack = 0;
        }

    }

    private void Start()
    {
        currState = stateMachine.GetComponent<TBCombatStateM>().currState;
        prevState = stateMachine.GetComponent<TBCombatStateM>().prevState;
        attack = stateMachine.GetComponent<TBCombatStateM>().atk;
        usedHealthPack = false;
        skillsMenu.SetActive(false);
    }

    public void OnSkillsPressed()
    {
        skillsMenu.SetActive(true);
    }

    public void OnItemPressed()
    {
        usedHealthPack = true;
        GameObject.Find("StateMachine").GetComponent<TBCombatStateM>().Player.hp = 1000;
    }

    public void OnJumpKickPressed()
    {
        attack = 1;
        skillsMenu.SetActive(false);
        prevState = TBCombatStateM.CombatState.PlayerSelect;
        currState = TBCombatStateM.CombatState.Combat;
    }

    public void OnSpinKickPressed()
    {
        attack = 2;
        skillsMenu.SetActive(false);
        prevState = TBCombatStateM.CombatState.PlayerSelect;
        currState = TBCombatStateM.CombatState.Combat;
    }
}
