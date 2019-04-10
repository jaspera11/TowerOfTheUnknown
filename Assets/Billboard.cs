using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Billboard : MonoBehaviour
{
    public GameObject camera;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position);
    }
}
