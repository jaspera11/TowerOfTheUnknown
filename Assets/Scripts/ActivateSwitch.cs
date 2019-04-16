using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to switch
public class ActivateSwitch : MonoBehaviour
{
    [SerializeField] private GameObject block = null;   // Object that activates switch
    public GameObject exit;                             // Object that player will exit through

    private void OnTriggerEnter(Collider other)
    {
        // If entering object is the block, then increment active switches
        if (GameObject.ReferenceEquals(other.gameObject, block))
            exit.GetComponent<LevelChange>().IncrementActiveSwitches();
    }

    private void OnTriggerExit(Collider other)
    {
        // If exiting object is the block, then decrement active switches
        if (GameObject.ReferenceEquals(other.gameObject, block))
            exit.GetComponent<LevelChange>().DecrementActiveSwitches();
    }
}
