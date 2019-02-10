using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Change : MonoBehaviour
{
    [SerializeField] Camera newCamera;
    [SerializeField] Camera oldCamera;
    [SerializeField] GameObject displayedObject;
    [SerializeField] GameObject disappearedObject;

    // Start is called before the first frame update
    void Start()
    {
        if (displayedObject != null)
            displayedObject.SetActive(false);
        if (disappearedObject != null)
            disappearedObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            oldCamera.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(true);
            if (displayedObject != null)
                displayedObject.SetActive(true);
            if (disappearedObject != null)
                disappearedObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            oldCamera.gameObject.SetActive(true);
            newCamera.gameObject.SetActive(false);
            if (displayedObject != null)
                displayedObject.SetActive(false);
            if (disappearedObject != null)
                disappearedObject.SetActive(true);
        }
    }
}
