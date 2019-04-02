using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Attach to object that player exits through
public class LevelChange : MonoBehaviour
{
    public int sceneIndex;
    public bool quitOnChange = false;
    public string condition;

    // Depends on condition
    public int requiredSwitches = 1;
    private int activatedSwitches = 0;

    // Also depends on condition
    private bool ChangeCondition(string condition)
    {
        switch (condition)
        {
            case "switches":
                return activatedSwitches >= requiredSwitches;
            default:
                return true;
        }
    }

    // When player walks through exit, quit or change to next level
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ChangeCondition(condition))
        {
            if (quitOnChange)
            {
                Application.Quit();
            }
            else
            {
                // Destroy all enemy objects (prevents redundancy)
                UnitStats.DestroyByTag("EnemyData");
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    // Increment/decrement activated switches
    public void IncrementActiveSwitches() { activatedSwitches++; }
    public void DecrementActiveSwitches() { activatedSwitches--; }
}
