using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class multipleChoiceScript : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject exit;

    public void Evaluator(bool isAnswer)
    {
        if (isAnswer)
        {
            TextBox.GetComponent<Text>().text = "Correct!";
            exit.GetComponent<LevelChange>().IncrementSolvedRiddles();
        }
        else
        {
            TextBox.GetComponent<Text>().text = "Wrong answer!";
            // [ Maybe some punishment ]
        }
    }

    /*
    void Update()
    {
        if (ChoiceMade >=1){
            Choice01.SetActive(false);
            Choice02.SetActive(false);
            Choice03.SetActive(false);
        }
        
    }
    */
}
