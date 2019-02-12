using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TBCombatStateM : MonoBehaviour
{
    
    private CombatStateInit CombatInitScript = new CombatStateInit();
    public Text StatusWindowText;
    public Text PlayerHPText;
    public Text PlayerSPText;
    public Text EnemyHPText;
    public int atk;

    public enum CombatState
    {
        Initial,
        PlayerSelect,
        Combat,
        EnemySelect,
        Victory
    }

    public CombatState currState;
    public CombatState prevState;
    public MainChar Player;
    public EnemyChar newEnemy;
    int rng;
    // Start is called before the first frame update
    void Start()
    {
        StatusWindowText.text = "";
        PlayerHPText.text = "";
        PlayerSPText.text = "";
        EnemyHPText.text = "";
        currState = CombatState.Initial;
        newEnemy = GetComponent<EnemyChar>();
        Player = GetComponent<MainChar>();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerHPText.text = "/1000";
        //Debug.Log(Player.hp);
        Debug.Log(currState);
        if (Player != null && PlayerHPText != null) { PlayerHPText.text = Player.hp.ToString() + "/1000"; }
        if (Player != null && PlayerSPText != null) { PlayerSPText.text = Player.sp.ToString() + "/100"; }
        if (newEnemy != null && EnemyHPText != null) { EnemyHPText.text = newEnemy.hp.ToString() + "/1000"; }
        switch (currState)
        {
            case (CombatState.Initial):
                //CombatInitScript.initBattle();
                
                currState = CombatState.PlayerSelect;
                break;
            case (CombatState.PlayerSelect):
                StatusWindowText.text = "Player's Turn...";
                break;
            case (CombatState.Combat):
                if (prevState == CombatState.PlayerSelect)
                {
                    StatusWindowText.text = "";
                    StatusWindowText.text = "Player made their move!";
                    if (atk == 0)
                    {
                        newEnemy.hp = newEnemy.hp - 100;
                    }
                    if (atk == 1)
                    {
                        newEnemy.hp = newEnemy.hp - 150;
                    }
                    if (atk == 2)
                    {
                        newEnemy.hp = newEnemy.hp - 200;
                    }
                    waitToCont();
                    if (newEnemy.hp <= 0)
                    {
                        newEnemy.hp = 0;
                        StatusWindowText.text = "";
                        StatusWindowText.text = "Enemy was defeated!";
                        currState = CombatState.Victory;
                    }
                    else { currState = CombatState.EnemySelect; }
                }
                if (prevState == CombatState.EnemySelect)
                {
                    rng = Random.Range(0,1);
                    if (rng == 0)
                    {
                        StatusWindowText.text = "";
                        StatusWindowText.text = "Enemy screamed!\n Player's crit chance decreased";
                    }
                    if (rng == 1)
                    {
                        StatusWindowText.text = "";
                        StatusWindowText.text = "Enemy threw a rock!";
                        Player.hp = Player.hp - 20;
                    }
                    waitToCont();
                    currState = CombatState.PlayerSelect;
                }
                break;
            case (CombatState.EnemySelect):
                StatusWindowText.text = "";
                StatusWindowText.text = "Enemy's Turn...";
                waitToCont();
                prevState = CombatState.EnemySelect;
                currState = CombatState.Combat;
                break;
            case (CombatState.Victory):
                StatusWindowText.text = "";
                StatusWindowText.text = "Player gained 100 exp!\n Player leveled up!";
                waitToCont();
                //Trigger Scene Transition
                break;
        }
    }
    private void OnGUI()
    {
        
    }
    public void Init()
    {
        
    }
    public IEnumerator waitToCont()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }
    
}
