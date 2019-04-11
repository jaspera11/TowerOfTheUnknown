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
    public GameObject PlayerStats;
    public GameObject EnemyStats;
    public GameObject Inventory;
    public Text StatusWindowText;
    public Text HealthText;
    public Text StaminaText;
    
    public int sceneIndex;
    private float runningChance;
    private bool isBossEncounter = false;
    
    
    private void Start()
    {
        SkillsList.SetActive(false);
        ItemList.SetActive(false);
        StatusWindowText.text = "Waiting...";
        HealthText.text = "";
        StaminaText.text = "";
        sceneIndex = 0;
        if (isBossEncounter == true) { runningChance = 0; }
        else { runningChance = (1 / 3); }
    }
    public void AttackButton()
    {
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            StatusWindowText.text = "Performed Attack";
            CombatStateMachine.GetComponent<CombatStateM>().movePower = 10;
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
        }
    }
    public void SkillsButton()
    {
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            SkillsList.SetActive(true);
            
            
        }
    }
    public void ItemsButton()
    {
        if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
        {
            HealthText.text = "Heal x" + (Inventory.GetComponent<Inventory>().heal);
            StaminaText.text = "Energy x" + (Inventory.GetComponent<Inventory>().energy);
            ItemList.SetActive(true);
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
                SceneManager.LoadScene(sceneIndex); //still needs work
            }
            else
            {
                StatusWindowText.text = "Retreat failed! There's no running anymore...";
                runningChance = 0;
            }
        }
    }
    public void toggledSkills()
    {

        if (PlayerStats.GetComponent<UnitStats>().stamina >= 10) {
            StatusWindowText.text = "Performed skill";
            CombatStateMachine.GetComponent<CombatStateM>().movePower = 30;
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
        }
        else
        {
            StatusWindowText.text = "Not enough energy!";
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
        }
        SkillsList.SetActive(false);
    }
    public void toggledSkills2()
    {
        if (PlayerStats.GetComponent<UnitStats>().stamina >= 5)
        {
            PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense * 2;
            StatusWindowText.text = "Performed skill";
        }
        else {
            StatusWindowText.text = "Not enough energy!";
            CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
        }
        SkillsList.SetActive(false);
    }
    public void toggledItems()
    {
        HealthText.text = "recovered Health!";
        Inventory.GetComponent<Inventory>().heal--;
        if (PlayerStats.GetComponent<UnitStats>().maxhealth > (PlayerStats.GetComponent<UnitStats>().health + 100)) {
            PlayerStats.GetComponent<UnitStats>().health += 100;
        }
        else
        {
            PlayerStats.GetComponent<UnitStats>().health = 200;
        }
        ItemList.SetActive(false);
    }
    public void toggledItems2()
    {
        HealthText.text = "recovered Stamina!";
        Inventory.GetComponent<Inventory>().energy--;
        if (PlayerStats.GetComponent<UnitStats>().maxstamina > (PlayerStats.GetComponent<UnitStats>().stamina + 10))
        {
            PlayerStats.GetComponent<UnitStats>().health += 10;
        }
        else
        {
            PlayerStats.GetComponent<UnitStats>().health = 50;
        }
        ItemList.SetActive(false);
    }
}
