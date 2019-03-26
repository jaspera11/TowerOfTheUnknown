using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BT;

namespace Combat
{
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
            //Implement base defense stat?
            if (PlayerStats.GetComponent<UnitStats>().defense > 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void basicAttack()
        {
            CombatStateMachine.GetComponent<CombatStateM>().movePower = 5;
            StatusWindowText.text = "The enemy strikes";
        }

        public void debuffDefense()
        {
            CombatStateMachine.GetComponent<CombatStateM>().movePower = 0;
            PlayerStats.GetComponent<UnitStats>().defense /= 2;
            StatusWindowText.text = "The enemy screeched, lowering your defense";
            //EnemyStats.GetComponent<UnitStats>().stamina -= 5;
        }

        public void shoot()
        {
            CombatStateMachine.GetComponent<CombatStateM>().movePower = 5;
            StatusWindowText.text = "The enemy fired their weapon!";
            EnemyStats.GetComponent<UnitStats>().stamina -= 10;
        }

        public bool enoughSP(int n)
        {
            if (EnemyStats.GetComponent<UnitStats>().stamina > n)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void chooseOption()
        {
            int[] rootWeights = { 40, 40, 20 };
            List<Node> level2 = new List<Node>();
            List<Node> level3_1 = new List<Node>();
            List<Node> level3_2 = new List<Node>();
            level2.Add(new Call(basicAttack));
            level3_1.Add(new Call(debuffDefense));
            level3_2.Add(new Call(shoot));
            level2.Add(new If(playerBuffedDefense, level3_1));
            level2.Add(new IfInt(enoughSP, 10, level3_2));
            RandomSelector Root = new RandomSelector(3, level2, rootWeights);
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

            if (Root.Evaluate() == Node.NodeStates.SUCCESS)
            {
                CombatStateMachine.GetComponent<CombatStateM>().currState = CombatStateM.CombatState.EnemyCombat;
            }

        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}