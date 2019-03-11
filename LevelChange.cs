using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public int sceneIndex;
    public bool quit = false;
    public GameObject switchObject;
    public int requiredSwitches;

    void OnTriggerEnter(Collider other)
    {
        //other.name should equal the root of your Player object
        if (other.CompareTag("Player") && GameObject.Find(switchObject.name).GetComponent<ActivateSwitch>().GetSwitchesActivated() >= requiredSwitches)
        {
            //The scene number to load (in File->Build Settings)
            
            if(quit)
            {
                Application.Quit();
            } else
            {
                SceneManager.LoadScene(sceneIndex);
            }
            
        }
    }
}