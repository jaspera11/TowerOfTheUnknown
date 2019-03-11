//Some code inspired by patrol code in Unity Documentation: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

public class Enemy_V2 : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] int battleScene = 1;
    [SerializeField] GameObject enemyPrefab;
    private bool spawning = false;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.spawning = true;

            //Load Battle scene when enemy encounters player.  Index may need to be edited
            SceneManager.LoadScene(battleScene);
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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Enemy prefab allows persistent enemy data
        if (scene.buildIndex == battleScene)
        {
            if (this.spawning)
            {
                Instantiate(enemyPrefab);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }
    }
}