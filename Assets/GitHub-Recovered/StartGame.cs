//Redundant

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    //[SerializeField] public int loadScene = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void startGame(int loadScene)
    {
        SceneManager.LoadScene(loadScene);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
