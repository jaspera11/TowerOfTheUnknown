using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openScene(int loadScene)
    {
        SceneManager.LoadScene(loadScene);
    }

    public void ShowItem(GameObject item)
    {
        
        if (!item.activeSelf)
        {
            item.SetActive(true);
        }
        else
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
