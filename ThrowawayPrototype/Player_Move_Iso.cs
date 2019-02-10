using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_Iso : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;
    Vector3 upDirection, rightDirection;

    // Start is called before the first frame update
    void Start()
    {
        upDirection = new Vector3(-1f, 0f, 0f);
        rightDirection = Quaternion.Euler(new Vector3(0, 90, 0)) * upDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) Move();
    }

    private void Move()
    {
        Vector3 rightMovement = rightDirection * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = upDirection * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        
        transform.forward = Vector3.Normalize(rightMovement + upMovement);  // Changes direction that Player is facing
        transform.position += rightMovement + upMovement;                   // Changes position of the Player
    }
}
