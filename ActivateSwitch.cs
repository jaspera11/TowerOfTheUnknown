using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour
{
    [SerializeField] private GameObject block = null;
    public GameObject exit;

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, block))
            GameObject.Find(exit.name).GetComponent<LevelChange>().incrementActiveSwitches();
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, block))
            GameObject.Find(exit.name).GetComponent<LevelChange>().decrementActiveSwitches(); ;
    }
}