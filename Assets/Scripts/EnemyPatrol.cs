using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code inspired from provided CubeWorld 3D Sandbox game
//Moves object between two points
public class EnemyPatrol : MonoBehaviour
{
    //Fields for user in engine
    [SerializeField] public float moveSpeed;
    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    [SerializeField] private Transform enemyObject;

    //Other member variables
    private bool reverse = false;
    private float iTime;
    private float totalDist;
    private float covDist;
    private float journeyProg;

    void Start()
    {
        iTime = Time.time;

        //objectToUse = transform;

        totalDist = Vector3.Distance(pointA.transform.position, pointB.transform.position);
    }



    void Update()
    {
        //Distance from start to present
        covDist = (Time.time - iTime) * moveSpeed;
        //Fractional progress towards destination
        journeyProg = covDist / totalDist;

        if (reverse)
        {
            //Move towards B
            enemyObject.position = Vector3.Lerp(pointB.transform.position, pointA.transform.position, journeyProg);
        }
        else
        {
            //Move towards A
            enemyObject.position = Vector3.Lerp(pointA.transform.position, pointB.transform.position, journeyProg);
        }

        //Change direction when the enemy reaches a point
        if ((Vector3.Distance(enemyObject.position, pointB.transform.position) == 0.0f || Vector3.Distance(enemyObject.position, pointA.transform.position) == 0.0f))
        {
            if (reverse)
            {
                reverse = false;
            }
            else
            {
                reverse = true;
            }

            //New initial time for new destination
            iTime = Time.time;
        }
    }
}