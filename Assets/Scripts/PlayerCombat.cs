using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CombatStateMachine;
    public Transform Atkbtn;
    public Transform Skillsbtn;
    public Transform Itemsbtn;
    public Transform Fleebtn;
    void Start()
    {

    }

    // Update is called once per frame
    public void disableButtons()
    {
        Atkbtn.GetComponent<Button>().interactable = false;
        Skillsbtn.GetComponent<Button>().interactable = false;
        Itemsbtn.GetComponent<Button>().interactable = false;
        Fleebtn.GetComponent<Button>().interactable = false;
    }
}
