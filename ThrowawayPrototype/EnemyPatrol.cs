using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
//Moves object between two points
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    [SerializeField] private Transform enemyObject;

    private bool reverseMove = false;
    private float startTime;
    private float journeyLength;
    private float distCovered;
    private float fracJourney;

    void Start()
    {
        startTime = Time.time;
        
        //objectToUse = transform;
        
        journeyLength = Vector3.Distance(pointA.transform.position, pointB.transform.position);
    }

    

    void Update()
    {
        distCovered = (Time.time - startTime) * moveSpeed;
        fracJourney = distCovered / journeyLength;

        if (reverseMove)
        {
            enemyObject.position = Vector3.Lerp(pointB.transform.position, pointA.transform.position, fracJourney);
        }
        else
        {
            enemyObject.position = Vector3.Lerp(pointA.transform.position, pointB.transform.position, fracJourney);
        }

        //Change direction when the enemy reaches a point
        if ((Vector3.Distance(enemyObject.position, pointB.transform.position) == 0.0f || Vector3.Distance(enemyObject.position, pointA.transform.position) == 0.0f))
        {
            if (reverseMove)
            {
                reverseMove = false;
            }
            else
            {
                reverseMove = true;
            }
            startTime = Time.time;
        }
    }
}