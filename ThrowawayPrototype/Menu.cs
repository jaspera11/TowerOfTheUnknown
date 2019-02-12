using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] public GameObject menu;

    [SerializeField] private bool menuOn = false;
    //private Rect rectL;
    //private GUIContent stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //void showMenu()
    //{
    //    menuOn = !menuOn;

    //    if(menuOn)
    //    {

    //    } else
    //    {
            
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("enter"))
        {
            menuOn = !menuOn;
        }

        //Show menu
        if(menuOn)
        {
            menu.SetActive(true);
        } else
        {
            menu.SetActive(false);
        }
    }
}
