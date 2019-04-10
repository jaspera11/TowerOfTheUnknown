using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int sceneIndex;
    public int requiredSwitches;
    private int activatedSwitches = 0;

    public void incrementActiveSwitches()
    {
        activatedSwitches++;
    }

    public void decrementActiveSwitches()
    {
        activatedSwitches--;
    }

    void OnTriggerEnter(Collider other)
    {
        //other.name should equal the root of your Player object
        if (other.CompareTag("Player") && activatedSwitches >= requiredSwitches)
        {
            //The scene number to load (in File->Build Settings)
            SceneManager.LoadScene(sceneIndex);

            //Debug.Log("I QUIT!");
            //Application.Quit();
        }
    }
}