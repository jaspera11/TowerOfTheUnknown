using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSwitch : MonoBehaviour
{
    [SerializeField] private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(door.GetComponent<LevelChange>().activatedSwitches == door.GetComponent<LevelChange>().requiredSwitches)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
