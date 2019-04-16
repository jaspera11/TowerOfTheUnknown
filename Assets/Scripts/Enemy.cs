// Some code inspired by patrol code in Unity Documentation: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

// Attach to enemy sprite
public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] int battleScene = 1;
    [SerializeField] public EnemyPrefab enemyData;
    // private Rigidbody rb;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        

        GotoNextPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //PlayerPrefab.gameData.prevSceneIndex = SceneManager.GetActiveScene().buildIndex;    // Saves the previous scene
            //PlayerPrefab.gameData.playerLocation = other.transform;                             // Saves player position in overworld
            PlayerPrefab.gameData.prevSceneIndex = SceneManager.GetActiveScene().buildIndex;    // Saves the previous scene
            PlayerPrefab.gameData.playerLocation = other.transform;                             // Saves player position in overworld
            Debug.Log("Collided with player");
            GameObject.Find("Players").GetComponent<PlayerPrefab>().enemyName = enemyData.name;
            SceneManager.LoadScene(battleScene);                                                // Load battle scene
        }
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyData == null)
        {
            Destroy(gameObject);
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}