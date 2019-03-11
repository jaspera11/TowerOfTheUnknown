using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * **IMPORTANT**: this.transform.position must properly be set so that it will be created in the correct position in CombatScene.
 */

public class PlayerInit : MonoBehaviour
{
    [SerializeField] int titleSceneIndex;
    [SerializeField] int combatSceneIndex = 1;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        this.gameObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == titleSceneIndex)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.SetActive(scene.buildIndex == combatSceneIndex);
        }
    }
}
