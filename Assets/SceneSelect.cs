using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void scene1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void scene2()
    {
        SceneManager.LoadScene("Level 1.1");
    }

}
