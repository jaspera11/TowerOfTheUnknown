using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private GameObject objectToEnable;

    void OnTriggerEnter(Collider other)
    {
        Destroy(objectToDestroy);
        objectToEnable.SetActive(true);
    }
}
