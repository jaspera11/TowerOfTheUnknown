using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour
{
    [SerializeField] private GameObject block = null;
    private int switchesActivated;

    // Start is called before the first frame update
    void Start()
    {
        switchesActivated = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, block))
            switchesActivated++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, block))
            switchesActivated--;
    }

    public int GetSwitchesActivated()
    {
        return switchesActivated;
    }
}
