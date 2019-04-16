using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * attack: option = attack, skill = new attack Skill
 * skills: option = skills, display skill menu
 * item: option = item, display item menu
 * flee: option = flee, end turn
 * "skill attack": skill = skillList.get("skill attack")
 * "item": item = inventory.remove("item")
 * enemy: display enemy stats, enemy = clicked enemy
 */

public class PlayerPhaseState : MonoBehaviour
{
    [SerializeField] private CombatStateM stateMachine;                      // Reference to state machine and variables
    [SerializeField] private const int attackPower = 10;    // The power of the move "Attack"
    //[SerializeField] private Text playerHealth, playerStamina, enemyHealth, enemyStamina;
    public Text chainconfirm1;
    public Text chainconfirm2;
    public Text chainconfirm3;
    public List<Text> items;
    public string holder;

    private void Update()
    {
        // TODO: [ Find a better place to put this ]
        // Updates health and stamina information
        //UnitStats player = (stateMachine.currState != CombatStateM.CombatState.EnemyPhase || stateMachine.currState != CombatStateM.CombatState.EnemyCombat)
        //    ? stateMachine.playerStats[stateMachine.uIndex] : stateMachine.enemyStats[stateMachine.uIndex];
        //playerHealth.text = player.health.ToString() + " / " + player.maxHealth.ToString();
        //playerStamina.text = player.stamina.ToString() + " / " + player.maxStamina.ToString();
        //enemyHealth.text = player.currEnemy.health.ToString() + " / " + player.currEnemy.maxHealth.ToString();
        //enemyStamina.text = player.currEnemy.stamina.ToString() + " / " + player.currEnemy.maxStamina.ToString();
        chainconfirm1.text = stateMachine.playerStats[stateMachine.uIndex].currSkills.Count + "/" + stateMachine.chainLength;
        chainconfirm2.text = stateMachine.playerStats[stateMachine.uIndex].currSkills.Count + "/" + stateMachine.chainLength;
        chainconfirm3.text = stateMachine.playerStats[stateMachine.uIndex].currSkills.Count + "/" + stateMachine.chainLength;
    }

    public void AttackButton()
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        if (stateMachine.currState != CombatStateM.CombatState.PlayerPhase)
            return;
        player.currOption = "Attack";
        stateMachine.AddBattleLog(player.charName + " is going to Attack!");
        player.currSkills.Push(new Skill("Attack", "Damage", attackPower, 0, 0));
    }

    public void SkillsButton()
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        if (stateMachine.currState != CombatStateM.CombatState.PlayerPhase)
            return;
        player.currOption = "Skills";
        GameObject currPanel = stateMachine.skillsPanels[stateMachine.uIndex];
        stateMachine.itemPanel.SetActive(false);
        currPanel.SetActive(!currPanel.activeSelf);
    }

    public void SetSkill(int index)
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        Skill skill = player.skillList[index];
        player.currSkills.Push(skill);
        stateMachine.AddBattleLog(player.charName + " is primed to use skill " + skill.name + "!");
    }

    public void SkillChain()
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        if (player.currSkills.Count <= 0)
        {
            return;
        }
        else
        {
            stateMachine.skillChainR = true;
        }
    }

    public void ItemButton()
    {
        int n = 0;
        foreach (Text item in items)
        {
            item.text = PlayerPrefab.gameData.inventory[n].name + " x" + PlayerPrefab.gameData.inventory[n].count;
            n++;
        }
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        if (stateMachine.currState != CombatStateM.CombatState.PlayerPhase)
            return;
        player.currOption = "Item";
        GameObject currPanel = stateMachine.itemPanel;
        stateMachine.skillsPanels[stateMachine.uIndex].SetActive(false);
        currPanel.SetActive(!currPanel.activeSelf);
    }

    public void SetItem(int index)
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        Item item = PlayerPrefab.gameData.inventory[index];
        player.currItem = item;
        item = PlayerPrefab.gameData.inventory[index] = new Item(item.name, item.count - 1);
        stateMachine.AddBattleLog(player.charName + " used item " + item.name + "!");
        //itemText.text = item.name + " x" + item.count;
    }

    public void FleeButton()
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        if (stateMachine.currState != CombatStateM.CombatState.PlayerPhase || stateMachine.fleeAttempted)
            return;
        player.currOption = "Flee";
    }

    public void SelectEnemy(int index)
    {
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        if (stateMachine.currState != CombatStateM.CombatState.PlayerPhase)
            return;
        player.currEnemy = stateMachine.enemyStats[index];
    }
    public void ptrenter(int index)
    {
        holder = stateMachine.battleLog.text;
        PlayerStats player = stateMachine.playerStats[stateMachine.uIndex];
        Skill skill = player.skillList[index];
        stateMachine.battleLog.fontSize = 120;
        stateMachine.battleLog.text = skill.name + ": \n" + "PWR: " + skill.power + " SP: " + skill.SPCost;
        switch (skill.name)
        {
            case "Psy-Throw":
                stateMachine.battleLog.text += "\nUses the power of telekinesis to jostle the enemy. A simple and weak attack.";
                break;
            case "Tear":
                stateMachine.battleLog.text += "\nUses the power of telekinesis to pull the enemy apart. A more advanced and powerful attack";
                break;
            case "Kinetic Flame":
                stateMachine.battleLog.text += "\nUses the power of telekinesis to create flames that are directed at enemies.";
                break;
            case "Kinetic Shock":
                stateMachine.battleLog.text += "\nUses the power of telekinesis to create a sphere of electricity.";
                break;
            case "Psy-Blast":
                stateMachine.battleLog.text += "\nA powerful concussive blast of psychic energy.";
                break;
            case "Thrown Edge":
                stateMachine.battleLog.text += "\nA thrown blade. Though weak it does catch enemies by surprise.";
                break;
            case "Mini Explosive":
                stateMachine.battleLog.text += "\nA tiny explosive that can be thrown at enemies.";
                break;
            case "Poisoned Smoke":
                stateMachine.battleLog.text += "\nThrow a canister of poisonous gas the corrodes away at those exposed to it.";
                break;
            case "Freezing Spray":
                stateMachine.battleLog.text += "\nA blast of freezing chill fire from a teched out gun.";
                break;
            case "Plasma Blaster":
                stateMachine.battleLog.text += "\nAn explosive plasma blast fired from teched out gun.";
                break;
            case "Straight Drive":
                stateMachine.battleLog.text += "\nA straight tackle performed at exceptional speeds";
                break;
            case "Rear Straight":
                stateMachine.battleLog.text += "\nA take down performed from behind at exceptional speeds";
                break;
            case "Straight Overdrive":
                stateMachine.battleLog.text += "\nA devastating superspeed tackle.";
                break;
            case "Rear Overdrive":
                stateMachine.battleLog.text += "\nA devastating superspeed rear takedown.";
                break;
            case "Supersonic Thrash":
                stateMachine.battleLog.text += "\nA barrage of powerful attacks at supersonic speeds.";
                break;
            case "Barrier":
                stateMachine.battleLog.text += "\nA protective shield created using telekinesis. Raises the party's defense.";
                break;
            case "Magnify":
                stateMachine.battleLog.text += "\nForce amplification using telekinesis. Increase the party's attack.";
                break;
            case "Gravity":
                stateMachine.battleLog.text += "\nArtificially increased gravity that restricts the enemy. Decreases the enemy team's defense";
                break;
            case "Bio-Drug":
                stateMachine.battleLog.text += "\nA top secret drug that accelerates healing. Recovers health for the whole team.";
                break;
            case "Smoke Bomb":
                stateMachine.battleLog.text += "\nA smoke grenade for diversions. Decreases the enemy team's critical chance";
                break;
            case "Flash":
                stateMachine.battleLog.text += "\nA bright light that disorients the enemy team decreasing their attack power.";
                break;
            case "Superspeed Guard":
                stateMachine.battleLog.text += "\nSuperspeed observation and reaction. Increases party's defense and decreases the enemys' attack.";
                break;
            case "Acceleration":
                stateMachine.battleLog.text += "\nShare the power of superspeed with allies. Increases the party's speed.";
                break;
            case "Speed Mirage":
                stateMachine.battleLog.text += "\nCreate illusions through movement faster than the eye can process. Increases party's crit chance and decreases enemies crit chance.";
                break;
            default:
                break;
        }
    }
    public void ptrexit()
    {
        stateMachine.battleLog.fontSize = 60;
        stateMachine.battleLog.text = holder;
    }
    public void itemptrenter(int index)
    {
        holder = stateMachine.battleLog.text;
        stateMachine.battleLog.fontSize = 120;
        switch (PlayerPrefab.gameData.inventory[index].name)
        {
            case "Heal":
                stateMachine.battleLog.text = "Recovers health completely. Use wisely.";
                break;
            case "Energy":
                stateMachine.battleLog.text = "Recovers some stamina.";
                break;
            case "Atk Boost":
                stateMachine.battleLog.text = "Enhances physique increasing attack power";
                break;
            case "Def Boost":
                stateMachine.battleLog.text = "Enhances physique increasing durability";
                break;
            case "Spd Boost":
                stateMachine.battleLog.text = "Enhances physique enhancing speed";
                break;
            case "Crit Boost":
                stateMachine.battleLog.text = "A strange item that can increase a person's luck";
                break;
        }
    }

}

/*
public void AttackButton()
{
    if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
    {
        StatusWindowText.text = "Performed Attack";
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 10;
        CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
    }
    isSkillPanelOn = false;
    SkillsList.SetActive(false);
    isItemPanelOn = false;
    ItemList.SetActive(false);
    SetOption("Attack");
}
public void SkillsButton()
{
    if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
    {
        if (isSkillPanelOn == false)
        {
            isSkillPanelOn = true;
            SkillsList.SetActive(true);
        }
        else { isSkillPanelOn = false; SkillsList.SetActive(false); }
    }
}
public void toggledDSkills1()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 10)
    {
        PlayerStats.GetComponent<UnitStats>().stamina -= 10;
        StatusWindowText.text = "Performed skill";
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 30;
        CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
    }
    else
    {
        StatusWindowText.text = "Not enough energy!";
        CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
    }
    isSkillPanelOn = false;
    SkillsList.SetActive(false);
    isItemPanelOn = false;
    ItemList.SetActive(false);
}
public void toggledDSkills2()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 10)
    {
        PlayerStats.GetComponent<UnitStats>().stamina -= 10;
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
public void toggledDSkills3()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 10)
    {
        PlayerStats.GetComponent<UnitStats>().stamina -= 10;
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
public void toggledDSkills4()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 10)
    {
        PlayerStats.GetComponent<UnitStats>().stamina -= 10;
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
public void toggledDSkills5()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 10)
    {
        PlayerStats.GetComponent<UnitStats>().stamina -= 10;
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
public void toggledUSkills1()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 5)
    {
        PlayerStats.GetComponent<UnitStats>().stamina -= 5;
        PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense * 2;
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 0;
        StatusWindowText.text = "Performed skill";
    }
    else
    {
        StatusWindowText.text = "Not enough energy!";
        CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
    }
    isSkillPanelOn = false;
    SkillsList.SetActive(false);
    isItemPanelOn = false;
    ItemList.SetActive(false);
}
public void toggledUSkills2()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 5)
    {
        PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense * 2;
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 0;
        StatusWindowText.text = "Performed skill";
    }
    else
    {
        StatusWindowText.text = "Not enough energy!";
        CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
    }
    SkillsList.SetActive(false);
}
public void toggledUSkills3()
{
    if (PlayerStats.GetComponent<UnitStats>().stamina >= 5)
    {
        PlayerStats.GetComponent<UnitStats>().defense = PlayerStats.GetComponent<UnitStats>().defense * 2;
        CombatStateMachine.GetComponent<CombatStateM>().movePower = 0;
        StatusWindowText.text = "Performed skill";
    }
    else
    {
        StatusWindowText.text = "Not enough energy!";
        CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
    }
    SkillsList.SetActive(false);
}
public void ItemButton()
{
    if (CombatStateMachine.GetComponent<CombatStateM>().currState == CombatStateM.CombatState.PlayerPhase)
    {
        if (isItemPanelOn == false)
        {
            HealthText.text = "Heal x" + (Inventory.GetComponent<Inventory>().heal);
            StaminaText.text = "Energy x" + (Inventory.GetComponent<Inventory>().energy);
            AtkbstText.text = "Atk Boost x" + (Inventory.GetComponent<Inventory>().atkbst);
            DefbstText.text = "Def Boost x" + (Inventory.GetComponent<Inventory>().defbst);
            SpdbstText.text = "Spd Boost x" + (Inventory.GetComponent<Inventory>().spdbst);
            CritbstText.text = "Crit Boost x" + (Inventory.GetComponent<Inventory>().critbst);
            if (Inventory.GetComponent<Inventory>().heal == 0) { HealthRecbtn.GetComponent<Button>().interactable = false; }
            else { HealthRecbtn.GetComponent<Button>().interactable = true; }
            if (Inventory.GetComponent<Inventory>().energy == 0) { StaminaRecbtn.GetComponent<Button>().interactable = false; }
            else { StaminaRecbtn.GetComponent<Button>().interactable = true; }
            if (Inventory.GetComponent<Inventory>().atkbst == 0) { Atkbstbtn.GetComponent<Button>().interactable = false; }
            else { Atkbstbtn.GetComponent<Button>().interactable = true; }
            if (Inventory.GetComponent<Inventory>().defbst == 0) { Defbstbtn.GetComponent<Button>().interactable = false; }
            else { Defbstbtn.GetComponent<Button>().interactable = true; }
            if (Inventory.GetComponent<Inventory>().spdbst == 0) { Spdbstbtn.GetComponent<Button>().interactable = false; }
            else { Spdbstbtn.GetComponent<Button>().interactable = true; }
            if (Inventory.GetComponent<Inventory>().critbst == 0) { Critbstbtn.GetComponent<Button>().interactable = false; }
            else { Critbstbtn.GetComponent<Button>().interactable = true; }
            isItemPanelOn = true;
            ItemList.SetActive(true);
        }
        else { isItemPanelOn = false; ItemList.SetActive(false); }
    }
}
public void toggledItems1()
{
    //Health Recovery
    StatusWindowText.text = "Recovered health!";
    Inventory.GetComponent<Inventory>().heal--;
    if (PlayerStats.GetComponent<UnitStats>().maxhealth > (PlayerStats.GetComponent<UnitStats>().health + 100))
    {
        PlayerStats.GetComponent<UnitStats>().health += 100;
    }
    else
    {
        PlayerStats.GetComponent<UnitStats>().health = PlayerStats.GetComponent<UnitStats>().maxhealth;
    }
    ItemList.SetActive(false);
}
public void toggledItems2()
{
    //Stamina Recovery
    StatusWindowText.text = "Recovered stamina!";
    Inventory.GetComponent<Inventory>().energy--;
    if (PlayerStats.GetComponent<UnitStats>().maxstamina > (PlayerStats.GetComponent<UnitStats>().stamina + 10))
    {
        PlayerStats.GetComponent<UnitStats>().stamina += 10;
    }
    else
    {
        PlayerStats.GetComponent<UnitStats>().stamina = PlayerStats.GetComponent<UnitStats>().maxstamina;
    }
    ItemList.SetActive(false);
}
public void toggledItems3()
{
    //Attack Boost
    StatusWindowText.text = "Attack power increased!";
    Inventory.GetComponent<Inventory>().atkbst--;
    ItemList.SetActive(false);
}
public void toggledItems4()
{
    //Defense Boost
    StatusWindowText.text = "Defenses Enhanced!";
    Inventory.GetComponent<Inventory>().defbst--;
    ItemList.SetActive(false);
}
public void toggledItems5()
{
    //Speed Boost
    StatusWindowText.text = "Speed was magnified!";
    Inventory.GetComponent<Inventory>().spdbst--;
    ItemList.SetActive(false);
}
public void toggledItems6()
{
    //Critical Boost
    StatusWindowText.text = "Feeling Lucky!";
    Inventory.GetComponent<Inventory>().critbst--;
    ItemList.SetActive(false);
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
    isSkillPanelOn = false;
    SkillsList.SetActive(false);
    isItemPanelOn = false;
    ItemList.SetActive(false);
}
public void endPlayerTurn()
{
    //Disable menu buttons
    Atkbtn.GetComponent<Button>().interactable = false;
    Skillsbtn.GetComponent<Button>().interactable = false;
    Itemsbtn.GetComponent<Button>().interactable = false;
    Fleebtn.GetComponent<Button>().interactable = false;
    //Turn off panels
    isSkillPanelOn = false;
    SkillsList.SetActive(false);
    isItemPanelOn = false;
    ItemList.SetActive(false);
    //Change to next state
    CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.PlayerCombat;
}
*/
