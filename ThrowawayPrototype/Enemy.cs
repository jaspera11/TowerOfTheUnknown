using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float moveTime = 0.1f;
    public bool up = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Move (int xDir, int yDir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector3 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector3 end = start + new Vector3(xDir, yDir);

        float distanceRemaining = (transform.position - end).magnitude;

        while (distanceRemaining > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, Time.deltaTime/moveTime);

            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            distanceRemaining = (transform.position - end).magnitude;

            
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(up)
        {
            Move(0, 5);
        } else
        {
            Move(0, -5);
        }

    }

}
