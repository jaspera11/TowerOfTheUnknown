using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnClick : MonoBehaviour
{
    //public bool show = false;
    //[SerializeField] public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowItem(GameObject item)
    {
        //show = !show;
        //Debug.Log("showItem called\n");
        if (!item.activeSelf)
        {
            item.SetActive(true);
            //Debug.Log("Showing Text\n");
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
