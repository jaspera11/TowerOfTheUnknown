using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    public float moveTime = 0.1f;
    private bool up = false;
    //public Transform PointA;
    //public Transform PointB;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void startMove(int xDir, int yDir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector3 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector3 end = start + new Vector3(xDir, yDir);

        StartCoroutine(Move(end));

    }

    IEnumerator Move (Vector3 end)
    {
        

        float distanceRemaining = (transform.position - end).sqrMagnitude;

        while (distanceRemaining > 0)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, Time.deltaTime / moveTime);

            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            distanceRemaining = (transform.position - end).sqrMagnitude;


            yield return null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enter battle if player
        if(collision.CompareTag("Player"))
        {
            //enter battle
            SceneManager.LoadScene("Battle");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            startMove(2, 1);
        }
        else
        {
            startMove(-2, -1);
        }
        up = !up;
    }

}
