using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] public GameObject menu;

    private bool menuOn = false;
    private bool statsOn = false;
    //private Rect rectL;
    //private GUIContent stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //void showStats()
    //{
    //    statsOn = !statsOn;

    //    if (statsOn)
    //    {

    //    }
    //    else
    //    {

    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            menuOn = !menuOn;
        }
        
        //Show menu
        if(menuOn)
        {
            menu.SetActive(true);
            Time.timeScale = 0f;
        } else
        {
            menu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
