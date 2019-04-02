﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to player overworld sprite
public class CharController : MonoBehaviour
{
    [SerializeField] float speed = 4f;  // Change in inspector to adjust move speed
    private bool playerCon = true;
    private bool ignoreUpdate = false;
    Vector3 ydir, xdir;                 // Keeps track of our relative forward and right vectors
    float facing;
    private Transform move;
    private Transform obj;
    private Rigidbody m_rigidbody;
    private bool facingRight;

    //private Transform player;


    void Start()
    {
        facing = Camera.main.transform.eulerAngles.y;

        ydir = Camera.main.transform.forward;   // Set forward to equal the camera's forward vector
        ydir.y = 0;                             // Make sure y is 0
        ydir = Vector3.Normalize(ydir);         // Make sure the length of vector is set to a max of 1.0
        xdir = Quaternion.Euler(new Vector3(0, 90, 0)) * ydir; // Set the right-facing vector to be facing right relative to the camera's forward vector
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        facingRight = true;

        move = transform;

        // Sets player location upon reverting to a scene (only works for level <-> battle)
        gameObject.transform.position = PlayerPrefab.gameData.playerLocation.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("space") && other.tag == "Movable")
        {
            // Debug.Log(move.name + " " + playerCon);
            obj = other.transform;
            move = playerCon ? obj : transform;
            playerCon = !playerCon;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ignoreUpdate = true;
    }
    private void OnTriggerExit(Collider other)
    {
        ignoreUpdate = false;
    }

    /*
     * if colliding
     * - turn it on
     * - turn it off
     * if not colliding
     * - turn it off
     */

    void Update()
    {
        if (Input.anyKey) // only execute if a key is being pressed
            Move();

        // ignore this if currently colliding
        if (Input.GetKeyDown("space") && !ignoreUpdate)
        {
            playerCon = true;
            move = transform;
        }

        if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && facingRight)
        {
            move.rotation = Quaternion.Euler(0, 270, 0);
            facingRight = false;
        }

        if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && !facingRight)
        {
            move.rotation = Quaternion.Euler(0, 90, 0);
            facingRight = true;
        }
    }

    void Move()
    {
        // Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // setup a direction Vector based on keyboard input. GetAxis returns a value between -1.0 and 1.0. If the A key is pressed, GetAxis(HorizontalKey) will return -1.0. If D is pressed, it will return 1.0
        Vector3 dir = new Vector3(-1 * Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        float facing = Camera.main.transform.eulerAngles.y;
        Vector3 dirCam = Quaternion.Euler(0, facing, 0) * dir;

        /*
        xdir = Quaternion.Euler(new Vector3(0, 90, 0)) * ydir;
        Vector3 xmov = xdir * speed * Time.deltaTime * Input.GetAxis("Horizontal"); // Our right movement is based on the right vector, movement speed, and our GetAxis command. We multiply by Time.deltaTime to make the movement smooth.
        Vector3 ymov = ydir * speed * Time.deltaTime * Input.GetAxis("Vertical"); // Up movement uses the forward vector, movement speed, and the vertical axis inputs.
        Vector3 heading = Vector3.Normalize(xmov + ymov); // This creates our new direction. By combining our right and forward movements and normalizing them, we create a new vector that points in the appropriate direction with a length no greater than 1.0
        */

        /*
        move.forward = heading; // Sets forward direction of our game object to whatever direction we're moving in
        Vector3 xmov = dir.x * speed * Time.deltaTime;
        Vector3 ymov = dir.y * speed * Time.deltaTime * Input.GetAxis("VerticalKey");
        move.position += xmov; // move our transform's position right/left
        move.position += ymov; // Move our transform's position up/down
        */

        move.position += dir * speed * Time.deltaTime;
        // Using dir not dirCam in project for now
    }
}
