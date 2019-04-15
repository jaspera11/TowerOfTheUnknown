using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Attach this to an empty game object (name doesn't matter, tagged as StateMachine)
public class CombatStateM : MonoBehaviour
{
    // [ Change some of these to public later... ]
    public List<PlayerStats> playerStats;  // List of player data (each unit)
    public List<UnitStats> enemyStats;     // List of enemy data (each unit)
    private List<float> expGain;            // List of experience gained by defeating each enemy
    public enum CombatState                // Possible combat states
    {
        Init,
        PlayerPhase,
        EnemyPhase,
        PlayerCombat,
        EnemyCombat,
        Win,
        Defeat
    }
    public CombatState currState;          // Current combat state
    public int uIndex;                     // Keeps track of the current unit (for move selection and combat)
    private bool mainCharDefeated;          // True if the main character (playerStats[0]) is defeated
    private bool fleeAttempted;             // True if a flee has been attempted for the battle

    // UI elements
    public List<GameObject> skillsPanels;
    public GameObject itemPanel;
    [SerializeField] private Text battleLog;
    [SerializeField] private Button attackButton, skillsButton, itemButton, fleeButton;
    [SerializeField] private List<Button> itemButtons;
    [SerializeField] private List<Button> skillButtons1, skillButtons2, skillButtons3;
    [SerializeField] private Text playerHealth, playerStamina, enemyHealth, enemyStamina;
    private List<List<Button>> skillButtons;

    /*
     * ~ Methods found ~
     * private void Start(): Initializes variables
     * private void Update(): Executes state machine once per frame
     * IEnumerator PlayerPhase(): Executes PlayerPhase actions, yields for decision making
     * private void PrePlayerPhaseState(): Variables set before PlayerPhase
     * private void PostPlayerPhaseState(): Variables set after PlayerPhase
     * private void UseSkill(UnitStats stats): Uses the top skill stored in currSkills
     * private void UseItem(PlayerStats stats): Uses the item stored in currItem
     * private void Damage(UnitStats stats, float movePower): Handles damage calculation, enemy death, and experience gain
     * private void AttemptFlee(): Attempt to flee the battle
     * private void Flee(): Handles player/enemy unit flee option
     * private float ExpValue(UnitStats stats): Calculates the experience value of an enemy unit
     * public void AddBattleLog(string textToAdd): Adds a message to the battle log
     */

    // Initialize variables
    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerPrefab>().players;  // References the persistent list of player units
        Debug.Log("Found players");
        enemyStats = GameObject.FindGameObjectWithTag("EnemyData").GetComponent<EnemyPrefab>().enemies;     // References the persistent list of enemy units
        Debug.Log("Found enemies");
        Debug.Log(enemyStats.Count);

        skillButtons = new List<List<Button>>();
        skillButtons.Add(skillButtons1);
        skillButtons.Add(skillButtons2);
        skillButtons.Add(skillButtons3);

        // TODO: [ Create enemy sprite buttons dynamically ]

        currState = CombatState.Init;                   // Init is the first state
        expGain = new List<float>();                    // List of experience gained from defeating each enemy
        mainCharDefeated = false;
        fleeAttempted = false;
        battleLog.text = "\n\n\n\n\n";          // Ingenius way of creating a battle log (assumes ~7~ newline text limit)
        // Sets skills/item panels to inactive upon battle start
        foreach (GameObject panel in skillsPanels)
        {
            panel.SetActive(false);
        }
        itemPanel.SetActive(false);
        uIndex = 0;

        // Initializes player and enemy properties
        foreach (PlayerStats player in playerStats)
        {
            player.stamina = player.maxStamina;         // Reset stamina after every battle
            player.itemUsed = false;                    // True if current player's item has been used (for that turn)
            player.currItem = new Item("", 0);          // String - name of the current item that player unit will use
            player.currOption = "";                     // String - name of the current option (Attack, Skills, Item, Flee)
            player.currSkills = new Stack<Skill>();     // Stack of Skills - skill(s) that player unit will use during combat phase
            player.currEnemy = enemyStats[0];                    // UnitStats - reference to the enemy unit that player unit is targeting
        }
        foreach (UnitStats enemy in enemyStats)
        {
            enemy.health = enemy.maxHealth;
            enemy.stamina = enemy.maxStamina;
            enemy.currOption = "";
            enemy.currSkills = new Stack<Skill>();
            enemy.currEnemy = playerStats[0];                     // UnitStats - reference to the player unit that enemy unit is targeting
            expGain.Add(ExpValue(enemy));               // Initializes list of experience values for each enemy unit
        }

        // [ Dialogue box... ]
        // Player1HP.text = "HP: " + PlayerStats.GetComponent<UnitStats>().health + "/" + PlayerStats.GetComponent<UnitStats>().maxhealth;
        // Player1SP.text = "SP: " + PlayerStats.GetComponent<UnitStats>().stamina + "/" + PlayerStats.GetComponent<UnitStats>().maxstamina;
        // Enemy1HP.text = "HP: " + EnemyStats.GetComponent<UnitStats>().health + "/" + EnemyStats.GetComponent<UnitStats>().maxhealth;
        // Enemy1SP.text = "SP: " + EnemyStats.GetComponent<UnitStats>().health + "/" + EnemyStats.GetComponent<UnitStats>().maxhealth;
    }

    private void Update()
    {
        // Main state controller
        switch (currState)
        {
            // Initial state
            case (CombatState.Init):
                AddBattleLog("The battle has begun!");
                // TODO: [Some animation/initialization text]
                currState = CombatState.PlayerPhase;
                break;

            // Player phase state (menu/move selection)
            case (CombatState.PlayerPhase):

                // If all units have gone, move to combat scene
                if (uIndex >= playerStats.Count)
                {
                    PostPlayerPhaseState(); // Sets variables after PlayerPhase state
                }
                // Else, make a decision for current player unit
                else
                {
                    //Debug.Log("In coroutine" + uIndex);
                    // TODO: [ Implement UI stuff, decision making (choose move, skill, enemy, etc.), use playerStats[uIndex] ]

                    //PlayerStats player = playerStats[uIndex];

                    itemButton.interactable = (playerStats[uIndex].itemUsed) ? false : true;    // itemUsed == true --> can't click on "Item"

                    // If count of item is 0, can't click on that item
                    // Items in inventory must have same index as items in item button list!! -------------- fixable later?
                    //Debug.Log("Before item button loop");
                    for (int i = 0; i < itemButtons.Count; i++)
                    {
                        //Debug.Log(i);
                        itemButtons[i].interactable = (GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerPrefab>().inventory[i].count > 0) ? true : false;
                    }

                    // If current player doesn't have the skill/stamina to use a skill, can't click on that skill
                    // Skills in each player's skill list must have same index as skills in skill button lists!! ------------- fixable later?
                    //Debug.Log("Before skill button loop");
                    for (int i = 0; i < skillButtons[uIndex].Count; i++)
                    {
                        //Debug.Log(i);
                        bool isInteractable = (playerStats[uIndex].skill >= playerStats[uIndex].skillList[i].skillReq)
                            && (playerStats[uIndex].stamina >= playerStats[uIndex].skillList[i].SPCost);
                        skillButtons[uIndex][i].interactable = (isInteractable) ? true : false;
                    }
                    //Debug.Log("After skill button loop");
                    playerStats[uIndex].currEnemy = enemyStats[0];  // Default current enemy is the first enemy

                    // Calculate how many skills player can use
                    int chainLength = 1;
                    for (int i = PlayerPrefab.gameData.spdTH.Length - 1; i >= 0; i--)
                    {
                        if (playerStats[uIndex].speed < PlayerPrefab.gameData.spdTH[i] || playerStats[uIndex].skill < PlayerPrefab.gameData.sklTH[i])
                        {
                            continue;
                        }
                        chainLength++;
                    }
                    //Debug.Log("Pre coroutine");
                    //StartCoroutine("PlayerPhase");

                    //while (deciding)
                    //{
                    //    Debug.Log("In deciding loop");
                    //    //yield return null;
                    //    deciding = (playerStats[uIndex].currOption.Length <= 0)
                    //        || (playerStats[uIndex].currOption == "Attack" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count == 0))
                    //        || (playerStats[uIndex].currOption == "Skills" && (playerStats[uIndex].currEnemy == null/* || playerStats[uIndex].currSkills.Count < chainLength*/))
                    //        || (playerStats[uIndex].currOption == "Item" && playerStats[uIndex].currItem.name == "");
                    //    Debug.Log("Deciding " + deciding);
                    //}
                    //PlayerPhase();
                    //Debug.Log("Post coroutine");
                    //Debug.Log(uIndex);

                    // If item chosen, use it, and then choose another option
                    if (playerStats[uIndex].currOption == "Item" && playerStats[uIndex].currItem.name != "")
                    {
                        //Debug.Log("Item check");
                        UseItem(playerStats[uIndex]);
                        playerStats[uIndex].currItem.name = "";
                        return;
                    }
                    playerStats[uIndex].itemUsed = false;   // Reverts itemUsed after turn ends


                    bool deciding = (playerStats[uIndex].currOption.Length <= 0)
                || (playerStats[uIndex].currOption == "Attack" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count == 0))
                || (playerStats[uIndex].currOption == "Skills" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count < chainLength))
                || (playerStats[uIndex].currOption == "Item" && playerStats[uIndex].currItem.name == "");    
                    //Debug.Log(deciding);
                    if (!deciding)
                    {
                        uIndex++;
                    }
                    //uIndex++;                               // Switches to next player

                    

                    
                }

                

                break;

            // Executes player decisions
            case (CombatState.PlayerCombat):
                // If all units have gone, it is the enemy's turn
                if (uIndex >= playerStats.Count)
                {
                    currState = CombatState.EnemyPhase;
                    uIndex = 0;
                }
                // Else, execute the player unit's decision
                else
                {
                    switch (playerStats[uIndex].currOption)
                    {
                        case "Attack":
                        case "Skills":
                            UseSkill(playerStats[uIndex]); // Use skills, damage enemies, gain exp if enemy defeated

                            // If player unit has no more skills, continue to next player unit
                            if (playerStats[uIndex].currSkills.Count <= 0)
                            {
                                uIndex++;
                            }
                            break;
                        case "Flee":
                            if (fleeAttempted) break;   // If flee attempted before, cannot try again
                            AttemptFlee();              // Attempt the flee
                            break;
                        default:
                            break;
                    }
                    // If there are no more enemies, player(s) win
                    if (enemyStats.Count <= 0)
                    {
                        currState = CombatState.Win;
                    }
                }
                break;

            // Enemy decision (use decision tree)
            case (CombatState.EnemyPhase):
                // If all enemy units have gone, move to combat scene
                if (uIndex >= enemyStats.Count)
                {
                    currState = CombatState.EnemyCombat;
                    uIndex = 0;
                }
                // Else, make a decision for current enemy unit
                else
                {
                    // TODO: [ Implement UI stuff, decision making (choose move, skill, target player, etc.), use enemyStats[uIndex] ]
                    ChooseOption();
                    uIndex++;
                }
                break;

            // Executes enemy decisions
            case (CombatState.EnemyCombat):
                // If all units have gone, it is the player's turn
                Debug.Log("Enemy combat, uIndex " + uIndex);
                if (uIndex >= enemyStats.Count)
                {
                    PrePlayerPhaseState();
                }
                // Else, execute the enemy unit's decision
                else
                {
                    switch (enemyStats[uIndex].currOption)
                    {
                        case "Attack":

                        case "Skills":
                            Debug.Log("Enemy using skill");
                            UseSkill(enemyStats[uIndex]);   // Use skills, damage enemies, gain exp if enemy defeated

                            // If enemy unit has no more skills, continue to next enemy unit
                            if (enemyStats[uIndex].currSkills.Count <= 0)
                            {
                                uIndex++;
                            }
                            break;
                        case "Flee":
                            Flee(); // If flee chosen, flee the battle
                            break;
                        default:
                            break;
                    }
                    // If there are no more players, you lose
                    if (playerStats.Count <= 0 || mainCharDefeated)
                    {
                        currState = CombatState.Defeat;
                    }
                }
                break;

            // Occurs when all enemies defeated
            case (CombatState.Win):
                // TODO: [ Do win animation ]
                AddBattleLog("You win!");

                // Reset variables
                currState = CombatState.Init;
                uIndex = 0;
                foreach (PlayerStats player in playerStats)
                {
                    player.stamina = player.maxStamina;
                }
                PlayerPrefab.gameData.isDeadEnemy[GameObject.FindGameObjectWithTag("EnemyData").name] = true;   // Enemy prefab object added to dead enemies list
                Destroy(GameObject.FindGameObjectWithTag("EnemyData"));                                         // Destroys enemy prefab (stays dead)
                SceneManager.LoadScene(PlayerPrefab.gameData.prevSceneIndex);                                   // Load previous scene
                break;

            // Occurs when all players defeated and/or main character defeated
            case (CombatState.Defeat):
                // TODO: [ Do defeat animation ]
                // TODO: [ Load game over screen - start over or from last save]
                AddBattleLog("You lose!");
                Destroy(GameObject.FindGameObjectWithTag("EnemyData"));     // Destroys enemy prefab (comes back)
                SceneManager.LoadScene(0);                                  // Game over scene
                break;
            default:
                break;
        }
        //Debug.Log(uIndex);


        playerHealth.text = playerStats[0].health.ToString() + " / " + playerStats[0].maxHealth.ToString();
        playerStamina.text = playerStats[0].stamina.ToString() + " / " + playerStats[0].maxStamina.ToString();
        enemyHealth.text = enemyStats[0].health.ToString() + " / " + enemyStats[0].maxHealth.ToString();
        enemyStamina.text = enemyStats[0].stamina.ToString() + " / " + enemyStats[0].maxStamina.ToString();
    }

    /*
     * -------------------------------------------------- METHODS ---------------------------------------------------------------
     */


    //Curently unused.  
    /*
     * Handles UI elements (State machine --> UI) and other calculations for each player unit
     * If player is still deciding, yield the CPU
     */
    IEnumerator PlayerPhase()
    {
        Debug.Log("In coroutine" + uIndex);
        // TODO: [ Implement UI stuff, decision making (choose move, skill, enemy, etc.), use playerStats[uIndex] ]

        //PlayerStats player = playerStats[uIndex];

        itemButton.interactable = (playerStats[uIndex].itemUsed) ? false : true;    // itemUsed == true --> can't click on "Item"

        // If count of item is 0, can't click on that item
        // Items in inventory must have same index as items in item button list!! -------------- fixable later?
        Debug.Log("Before item button loop");
        for (int i = 0; i < itemButtons.Count; i++)
        {
            Debug.Log(i);
            itemButtons[i].interactable = (GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerPrefab>().inventory[i].count > 0) ? true : false;
        }

        // If current player doesn't have the skill/stamina to use a skill, can't click on that skill
        // Skills in each player's skill list must have same index as skills in skill button lists!! ------------- fixable later?
        Debug.Log("Before skill button loop");
        for (int i = 0; i < skillButtons[uIndex].Count; i++)
        {
            //Debug.Log(i);
            bool isInteractable = (playerStats[uIndex].skill >= playerStats[uIndex].skillList[i].skillReq)
                && (playerStats[uIndex].stamina >= playerStats[uIndex].skillList[i].SPCost);
            skillButtons[uIndex][i].interactable = (isInteractable) ? true : false;
        }
        Debug.Log("After skill button loop");
        playerStats[uIndex].currEnemy = enemyStats[0];  // Default current enemy is the first enemy

        // Calculate how many skills player can use
        int chainLength = 1;
        for (int i = PlayerPrefab.gameData.spdTH.Length - 1; i >= 0; i--)
        {
            if (playerStats[uIndex].speed < PlayerPrefab.gameData.spdTH[i] || playerStats[uIndex].skill < PlayerPrefab.gameData.sklTH[i])
            {
                continue;
            }
            chainLength++;
        }

        // Main boolean that checks if the player unit has completed their turn
        bool deciding = (playerStats[uIndex].currOption.Length <= 0)
                || (playerStats[uIndex].currOption == "Attack" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count == 0))
                || (playerStats[uIndex].currOption == "Skills" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count < chainLength))
                || (playerStats[uIndex].currOption == "Item" && playerStats[uIndex].currItem.name == "");
        Debug.Log(deciding);
        while (deciding)
        {
            Debug.Log("In deciding loop");
            yield return null;
            deciding = (playerStats[uIndex].currOption.Length <= 0)
                || (playerStats[uIndex].currOption == "Attack" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count == 0))
                || (playerStats[uIndex].currOption == "Skills" && (playerStats[uIndex].currEnemy == null || playerStats[uIndex].currSkills.Count < chainLength))
                || (playerStats[uIndex].currOption == "Item" && playerStats[uIndex].currItem.name == "");
            Debug.Log("Deciding " + deciding);
        }

        // Deactivates the skills/items panels for each player unit after their turn
        foreach (GameObject panel in skillsPanels)
        {
            panel.SetActive(false);
        }
        itemPanel.SetActive(false);
    }

    // Executed before PlayerPhase state
    private void PrePlayerPhaseState()
    {
        foreach (PlayerStats player in playerStats)
        {
            player.currItem = new Item("", 0);          // Item - current item that player will use
            player.currOption = "";                     // String - name of the current option (Attack, Skills, Item, Flee)
            player.currSkills = new Stack<Skill>();     // Stack of Skills - skill(s) that player unit will use during combat phase
            player.currEnemy = null;                    // UnitStats - reference to the enemy unit that player unit is targeting
        }
        // Make buttons interactable (if flee has been attempted, make that uninteractable
        attackButton.interactable = skillsButton.interactable = itemButton.interactable = true;
        fleeButton.interactable = (fleeAttempted) ? false : true;
        currState = CombatState.PlayerPhase;
        uIndex = 0;
    }

    // Executed after PlayerPhase state
    private void PostPlayerPhaseState()
    {
        // Make buttons uninteractable
        attackButton.interactable = skillsButton.interactable = itemButton.interactable = fleeButton.interactable = false;
        currState = CombatState.PlayerCombat;
        uIndex = 0;
    }

    // Use the top skill in the currSkill stack and executes it
    private void UseSkill(UnitStats stats)
    {
        // If there are no skills in the currSkills stack, exit the function
        if (stats.currSkills.Count <= 0)
        {
            return;
        }

        Skill skill = stats.currSkills.Pop();       // Get the top skill and remove it from the stack
        stats.stamina -= skill.SPCost;              // Subtracts SPCost from unit's stamina
        switch (skill.type)
        {
            // If it's a damaging skill, damage (and possibly defeat) the opponent, gain experience
            case "Damage":
                Damage(stats, skill.power);
                break;
            // If it's a utility skill, use a custom effect
            case "Utility":
                switch (skill.name)
                {
                    // TODO: [ Fill in cases as necessary for what to do for utility skills ]
                    // [ Placeholder ]
                    case "Barrier":
                        //increase party defense
                        for (int i = 0; i < playerStats.Count; i++)
                        {
                            playerStats[i].defense *= 2;
                        }
                        AddBattleLog("The team's defense has doubled!");
                        break;
                    case "Magnify":
                        //increase party attack
                        for(int i=0; i<playerStats.Count;i++)
                        {
                            playerStats[i].attack += 10;
                        }
                        AddBattleLog("The team's attack has increased!");
                        break;
                    case "Gravity":
                        //decrease enemy party defense
                        for(int i = 0; i < enemyStats.Count; i++)
                        {
                            enemyStats[i].defense = Mathf.Ceil(enemyStats[i].defense /2);
                        }
                        AddBattleLog("The enemy team's defense has decreased!");
                        break;
                    case "":
                        //increase party defense, decrease enemy party attack
                        for (int i = 0; i < playerStats.Count; i++)
                        {
                            playerStats[i].defense *= 2;
                        }
                        for (int i = 0; i < enemyStats.Count; i++)
                        {
                            enemyStats[i].attack = Mathf.Ceil(enemyStats[i].attack / 2);
                        }
                        AddBattleLog("Your party's defense has increased and the enemy team's attack has decreased!");
                        break;
                    case " ":
                        //increase party speed
                        for (int i = 0; i < playerStats.Count; i++)
                        {
                            playerStats[i].speed *= 2;
                        }
                        AddBattleLog("The team's speed has increased!");
                        break;

                    case "  ":
                        //Increase party's luck, decrease enemy's luck
                        for (int i = 0; i < playerStats.Count; i++)
                        {
                            playerStats[i].luck *= 2;
                        }
                        for (int i = 0; i < enemyStats.Count; i++)
                        {
                            enemyStats[i].luck = Mathf.Ceil(enemyStats[i].luck / 2);
                        }
                        AddBattleLog("Your party's crit chance has increased and the enemy team's crit chance has decreased!");
                        break;
                    case "Bio-Drug":
                        //Heal party
                        for (int i = 0; i < playerStats.Count; i++)
                        {
                            playerStats[i].health += (int).33* playerStats[i].health;
                        }
                        AddBattleLog("The team's health has recovered!");
                        break;
                    case "Smoke Bomb":
                        //Decrease enemy crit chance
                        for (int i = 0; i < enemyStats.Count; i++)
                        {
                            enemyStats[i].luck = Mathf.Ceil(enemyStats[i].luck / 2);
                        }
                        AddBattleLog("The enemy team's crit chance has decreased!");
                        break;
                    case "Flash":
                        //Decrease enemy party attack
                        for (int i = 0; i < enemyStats.Count; i++)
                        {
                            enemyStats[i].attack = Mathf.Ceil(enemyStats[i].attack / 2);
                        }
                        AddBattleLog("The enemy team's attack has decreased!");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        // TODO: [ Do skill animation ]
    }

    // Use the item in currItem
    private void UseItem(PlayerStats stats)
    {
        // TODO: [ Fill in cases as necessary for what to do for each item ]
        // TODO: [ Do animation for each item]
        switch (stats.currItem.name)
        {
            // [ Placeholder ]
            case "Heal":
                stats.health = stats.maxHealth;
                AddBattleLog("You fully recovered health!");
                break;
            case "Energy":
                stats.stamina += (int).25 * stats.maxStamina;
                break;
            case "Atk Boost":
                stats.attack += 10;
                break;
            case "Def Boost":
                stats.defense += 10;
                break;
            case "Spd Boost":
                stats.speed += 10;
                break;
            case "Crit Boost":
                stats.luck += 10;
                break;
            default:
                break;
        }
        stats.itemUsed = true;  // Player has used the item (cannot use it again until the next turn)
    }

    /* 
     * Handles damage calculation.
     * If damage is higher than enemy health, remove enemy from list
     * If it's a player unit, gain experience and level up if there's enough experience
     */
    private void Damage(UnitStats stats, float movePower)
    {
        float attack = stats.attack;                // Current player attack
        float defense = stats.currEnemy.defense;    // Current enemy defense
        float luck = stats.luck;                    // Current player luck
        float critChance = (Mathf.Log(luck + 100, 1.05f) + 1 - Mathf.Log(100, 1.05f)) / 100f;   // Chance of a critical hit
        float critFactor = (Random.Range(0f, 1f) < critChance) ? 1.5f : 1f;                     // Multiplier (results from critical hit), 1.5x damage
        float random = Random.Range(0f, 1f) / 10f + 0.95f;                                      // Varies damage calculation with random factor
        int damage = (int)Mathf.Ceil(attack / defense * movePower * critFactor * random);       // Damage calculation

        // TODO: [ Do damage animation ]
        AddBattleLog(stats.currEnemy.charName + " took " + damage + " damage!");

        // If attack kills enemy, remove player/enemy from list
        if (damage >= stats.currEnemy.health)
        {
            stats.currEnemy.health = 0; // Health == 0 --> player/enemy is defeated
            AddBattleLog(stats.currEnemy.charName + " was defeated!");
            // If it's the player's turn, remove the enemy and gain experience
            if (currState == CombatState.PlayerCombat)
            {
                UnitStats enemy = stats.currEnemy;              // Gets the enemy unit
                float exp = expGain[enemyStats.IndexOf(enemy)]; // Gets the enemy exp value from a list (calculated at beginning)
                enemyStats.Remove(enemy);                       // Removes enemy unit from enemyStats list
                AddBattleLog("Players gained " + exp + " experience!");
                // Have (all) the players gain experience, level up if necessary
                foreach (PlayerStats player in playerStats)
                {
                    player.AddExperience(exp);
                }
            }
            // If it's the enemy's turn, remove that player unit from the list
            else
            {
                PlayerStats player = playerStats.Find(p => p.charName == stats.currEnemy.charName);
                AddBattleLog(player.charName + " was defeated!");
                // If the main character is defeated, you lose
                if (playerStats.IndexOf(player) == 0)
                {
                    mainCharDefeated = true;
                }
                else
                {
                    playerStats.Remove(player);
                }
            }
        }
        // If the enemy is not defeated, just damage the enemy
        else
        {
            stats.currEnemy.health -= damage;
        }
    }

    // Attempt to flee the battle
    private void AttemptFlee()
    {
        // If enemy is a boss, you can't run
        float fleeChance = (GameObject.FindGameObjectWithTag("EnemyData").GetComponent<EnemyPrefab>().isBoss) ? 0f : (1f / 3f);
        if (Random.Range(0f, 1f) < fleeChance) Flee(); // If flee chosen, flee the battle
        fleeAttempted = true;
    }

    // Handles player/enemy unit flee option
    private void Flee()
    {
        // TODO: [ Do flee animation ]
        AddBattleLog("Player fled!");

        // Reset variables
        currState = CombatState.Init;
        uIndex = 0;
        foreach (PlayerStats player in playerStats)
        {
            player.stamina = player.maxStamina;
        }
        foreach (UnitStats enemy in enemyStats)
        {
            enemy.health = enemy.maxHealth;
            enemy.stamina = enemy.maxStamina;
        }
        Destroy(GameObject.FindGameObjectWithTag("EnemyData"));
        SceneManager.LoadScene(PlayerPrefab.gameData.prevSceneIndex);   // Load previous (non-battle) scene
    }

    // Calculates the amount of experience the player(s) gain after defeating an enemy
    private float ExpValue(UnitStats stats)
    {
        return stats.level * 100f + stats.health + stats.stamina + stats.attack + stats.defense + stats.speed + stats.luck;
    }

    // Adds a message to the main text battle log
    public void AddBattleLog(string textToAdd)
    {
        string text = battleLog.text;
        battleLog.text = string.Concat(text.Remove(0, text.IndexOf("\n") + 1), textToAdd, "\n");
    }

    public void ChooseSkills()
    {
        for (int i = 0; i < enemyStats[uIndex].skillList.Count; i++)
        {
            if (CheckSkill(i))
            {
                AddSkill(i);
                enemyStats[uIndex].currOption = "Skills";
                AddBattleLog("The enemy attacked with " + enemyStats[uIndex].skillList[i].name + "!");
                return;
            }
        }
    }

    public void AddSkill(int ind)
    {
        enemyStats[uIndex].currSkills.Push(enemyStats[uIndex].skillList[ind]);
    }

    public bool CheckSkill(int ind)
    {
        //Debug.Log("uIndex when checking skill " + uIndex);
        if (enemyStats[uIndex].stamina >= enemyStats[uIndex].skillList[ind].SPCost)
            return true;
        else
            return false;
    }

    public void SetTarget()
    {
        int n = playerStats.Count;
        double[] weight = new double[3]; //Currently not used
        int index = 0;
        int rand = Random.Range(0, 101);
        switch (n)
        {
            case 1:
                weight[0] = 1;
                index = 0;
                break;
            case 2:
                if (rand > 50)
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
                weight[0] = .5;
                weight[1] = .5;
                break;
            case 3:
                if (rand > 70)
                {
                    index = 2;
                }
                else if (rand > 40)
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
                weight[0] = .4;
                weight[1] = .3;
                weight[2] = .3;
                break;

        }

        enemyStats[uIndex].currEnemy = playerStats[index];
    }

    public void BasicAttack()
    {
        //CombatStateMachine.GetComponent<CombatStateM>().movePower = 5;
        enemyStats[uIndex].currSkills.Push(new Skill("Attack", "Damage", 10, 0, 0));
        enemyStats[uIndex].currOption = "Attack";
        AddBattleLog("The enemy strikes");
        Debug.Log("Enemy basic attack");
    }

    public void ChooseOption()
    {
        //EnemyStats = enemy;
        SetTarget();
        AddBattleLog("The Enemy is Contemplating...");
        //PlayerStats playerTarget = new PlayerStats();

        int[] rootWeights = { 30, 70 };

        //List<Node> targets = new List<Node>();
        //targets.Add(new Target(setTarget, playerTarget));
        //targets.Add(new Target(setTarget, playerTarget));
        //targets.Add(new Target(setTarget, playerTarget));
        //Selector target = new Selector(targets);

        List<Node> level2 = new List<Node>();
        List<Node> level3_1 = new List<Node>();
        List<Node> level3_2 = new List<Node>();
        level3_1.Add(new Call(ChooseSkills));
        level2.Add(new IfInt(CheckSkill, 0, level3_1));
        level2.Add(new Call(BasicAttack));
        //level3_1.Add(new Call(DebuffDefense));
        //level3_2.Add(new Call(Shoot));
        //level2.Add(new If(PlayerBuffedDefense, level3_1));

        //RandomSelector Root = new RandomSelector(2, level2, rootWeights);
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

        //if (target.Evaluate() == Node.NodeStates.SUCCESS)
        //{
        Debug.Log("pre root evaluate");
        if (Root.Evaluate() == Node.NodeStates.SUCCESS)
        {
            //currState = CombatState.EnemyCombat;
        }
        //}


    }
}