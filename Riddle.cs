using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Riddle : MonoBehaviour
{
    [SerializeField] public InputField answerField;
    [SerializeField] public GameObject door;
    [SerializeField] public string answer;
    //[SerializeField] public int part;

    private void Start()
    {
        door.SetActive(false);
        //answerField.SetActive(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        answerField.SetActive(true);
    //    }
    //}

    void Update()
    {
        if(answerField.text == answer)
        {
            //Do thing
            //switch(part) {
            //    case 1:

            //        break;
            //    case 2:

            //        break;

            //    case 3:
            //        door.SetActive(true);
            //        break;
            //    default:

            //        break;

            //}
            door.SetActive(true);
        }
    }
}
