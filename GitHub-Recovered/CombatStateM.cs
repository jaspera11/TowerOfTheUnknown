using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Attach this to an empty game object (name doesn't matter, tagged as StateMachine)
public class CombatStateM : MonoBehaviour
{
    // [ Change some of these to public later... ]
    private List<PlayerStats> playerStats;  // List of player data (each unit)
    private List<UnitStats> enemyStats;     // List of enemy data (each unit)
    private List<float> expGain;            // List of experience gained by defeating each enemy
    private enum CombatState                // Possible combat states
    {
        Init,
        PlayerPhase,
        EnemyPhase,
        PlayerCombat,
        EnemyCombat,
        Win,
        Defeat
    }
    private CombatState currState;          // Current combat state
    private int uIndex;                     // Keeps track of the current unit (for move selection and combat)
    private bool mainCharDefeated;          // True if the main character (playerStats[0]) is defeated

    // Initialize variables
    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerPrefab>().players;  // References the persistent list of player units
        enemyStats = GameObject.FindGameObjectWithTag("EnemyData").GetComponent<EnemyPrefab>().enemies;     // References the persistent list of enemy units
        currState = CombatState.Init;       // Init is the first state
        expGain = new List<float>();
        mainCharDefeated = false;
        uIndex = 0;

        // Initializes player and enemy properties
        foreach (PlayerStats player in playerStats)
        {
            player.stamina = player.maxStamina;         // Reset stamina after every battle
            player.itemUsed = false;                    // True if current player's item has been used (for that turn)
            player.currItem = "";                       // String - name of the current item that player unit will use
            player.currOption = "";                     // String - name of the current option (Attack, Skills, Item, Flee)
            player.currSkills = new Stack<Skill>();     // Stack of Skills - skill(s) that player unit will use during combat phase
            player.currEnemy = null;                    // UnitStats - reference to the enemy unit that player unit is targeting
        }
        foreach (UnitStats enemy in enemyStats)
        {
            enemy.health = enemy.maxHealth;
            enemy.stamina = enemy.maxStamina;
            enemy.currOption = "";
            enemy.currSkills = new Stack<Skill>();
            enemy.currEnemy = null;                     // UnitStats - reference to the player unit that enemy unit is targeting
            expGain.Add(ExpValue(enemy));               // Initializes list of experience values for each enemy unit
        }

        // [Dialogue box...]
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
                // [Some animation/initialization text]
                currState = CombatState.PlayerPhase;
                break;

            // Player phase state (menu/move selection)
            case (CombatState.PlayerPhase):
                // If all units have gone, move to combat scene
                if (uIndex >= playerStats.Count)
                {
                    currState = CombatState.PlayerCombat;
                    uIndex = 0;
                }
                // Else, make a decision for current player unit
                else
                {
                    // [ Implement UI stuff, decision making (choose move, skill, enemy, etc.), use playerStats[uIndex] ]
                    // [ itemUsed == true --> can't click on "Item" ]

                    // [ If player's skill is less than skill's skill requirement --> can't click on that skill item ]
                    // [ If player's stamina is less than SPCost --> can't click on that skill item ]

                    // If item chosen, use it, and then choose another option
                    if (playerStats[uIndex].currOption == "Item")
                    {
                        UseItem(playerStats[uIndex]);
                        break;
                    }
                    playerStats[uIndex].itemUsed = false;   // Reverts itemUsed after turn ends
                    uIndex++;                               // Switches to next player
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
                            

                            // [ Do animation ]

                            // If player unit has no more skills, continue to next player unit
                            if (playerStats[uIndex].currSkills.Count <= 0)
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
                    // [ Implement UI stuff, decision making (choose move, skill, target player, etc.), use enemyStats[uIndex] ]
                    uIndex++;
                }
                break;

            // Executes enemy decisions
            case (CombatState.EnemyCombat):
                // If all units have gone, it is the player's turn
                if (uIndex >= enemyStats.Count)
                {
                    currState = CombatState.PlayerPhase;
                    uIndex = 0;
                }
                // Else, execute the enemy unit's decision
                else
                {
                    switch (enemyStats[uIndex].currOption)
                    {
                        case "Attack":
                        case "Skills":
                            UseSkill(enemyStats[uIndex]);   // Use skills, damage enemies, gain exp if enemy defeated

                            // [ Do animation ]

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
                // Reset variables
                currState = CombatState.Init;
                uIndex = 0;
                foreach (PlayerStats player in playerStats)
                {
                    player.stamina = player.maxStamina;
                }
                PlayerPrefab.gameData.isDeadEnemy[GameObject.FindGameObjectWithTag("EnemyData").name] = true;   // Enemy prefab object added to dead enemies list
                SceneManager.LoadScene(PlayerPrefab.gameData.prevSceneIndex);                                   // Load previous scene
                break;

            // Occurs when all players defeated and/or main character defeated
            case (CombatState.Defeat):
                // [ Load game over screen - start over or from last save]
                SceneManager.LoadScene(0);  // Game over scene
                break;
            default:
                break;
        }
    }

    // Use the top skill in the currSkill stack and executes it
    private void UseSkill(UnitStats stats)
    {
        // If there are no skills in the currSkills stack, exit the function
        if (stats.currSkills.Count <= 0)
        {
            return;
        }

        // Get the top skill and remove it from the stack
        Skill skill = stats.currSkills.Pop();
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
                    // [ Fill in cases as necessary for what to do for utility skills ]
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        // [ Animation for the skill ]
    }

    // Use the item in currItem
    private void UseItem(PlayerStats stats)
    {
        // [ Fill in cases as necessary for what to do for each item ]
        switch (stats.currItem)
        {
            default:
                break;
        }
        stats.itemUsed = true;  // Player has used the item (cannot use it again until the next turn)
    }

    // Handles player/enemy unit flee option
    private void Flee()
    {
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
        SceneManager.LoadScene(PlayerPrefab.gameData.prevSceneIndex);   // Load previous (non-battle) scene
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
        float critFactor = (Random.Range(0, 1) < critChance) ? 1.5f : 1f;                       // Multiplier (results from critical hit), 1.5x damage
        float random = Random.Range(0, 1) / 10f + 0.95f;                                        // Varies damage calculation with random factor
        int damage = (int)Mathf.Ceil(attack / defense * movePower * critFactor * random);       // Damage calculation
        // If attack kills enemy, remove player/enemy from list
        if (damage >= stats.currEnemy.health)
        {
            stats.currEnemy.health = 0; // Health == 0 --> player/enemy is defeated
            // If it's the player's turn, remove the enemy and gain experience
            if (currState == CombatState.PlayerCombat)
            {
                UnitStats enemy = stats.currEnemy;              // Gets the enemy unit
                float exp = expGain[enemyStats.IndexOf(enemy)]; // Gets the enemy exp value from a list (calculated at beginning)
                enemyStats.Remove(enemy);                       // Removes enemy unit from enemyStats list
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

    // Calculates the amount of experience the player(s) gain after defeating an enemy
    private float ExpValue(UnitStats stats)
    {
        return stats.level * 100f + stats.health + stats.stamina + stats.attack + stats.defense + stats.speed + stats.luck;
    }
}

// ----------------------------------------------------------------------------

/*
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
    Player1HP.text = "HP: " + PlayerStats.GetComponent<UnitStats>().health + "/" + PlayerStats.GetComponent<UnitStats>().maxhealth;
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



public int calculateExperience(UnitStats enemyStats, int oldExp)
{
    return oldExp + enemyStats.health + enemyStats.stamina + enemyStats.attack + enemyStats.defense + enemyStats.speed + enemyStats.luck;
}
*/
