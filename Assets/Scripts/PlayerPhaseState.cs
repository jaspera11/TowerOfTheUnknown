using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPhaseState : MonoBehaviour
{
    public GameObject SkillsList;
    public GameObject ItemList;
    public GameObject CombatStateMachine;
    public Text StatusWindowText;
    private float runningChance;
    public GameObject EnemyStats;
    public int recovery = 10;
    public Text HealthText;
    public GameObject PlayerStats;
    private void Start()
    {
        SkillsList.SetActive(false);
        ItemList.SetActive(false);
        runningChance = (1/3);
        StatusWindowText.text = "Waiting...";
        HealthText.text = "";
    }
    public void AttackButton()
    {
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            StatusWindowText.text = "Performed Attack";
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
        }
    }
    public void SkillsButton()
    {
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            SkillsList.SetActive(true);
            
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
        }
    }
    public void ItemsButton()
    {
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            ItemList.SetActive(true);
            //Method to select an item to use.
        }
    }
    public void FleeButton()
    {
        //flee stuff
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            float randomNumber = Random.value;
            if (randomNumber < this.runningChance)
            {
               // SceneManager.LoadScene(/*"OverworldScene"*/);
            }
            else
            {
                this.runningChance = 0;
            }
        }
    }
    public void toggledSkills()
    {
        EnemyStats.GetComponent<UnitStats>().health = EnemyStats.GetComponent<UnitStats>().health - 30;
        StatusWindowText.text = "Performed skill";
    }
    public void toggledItems()
    {
        HealthText.text = "HPRET x" + recovery;
        recovery--;
        PlayerStats.GetComponent<UnitStats>().health = PlayerStats.GetComponent<UnitStats>().health + 100;
    }
}
