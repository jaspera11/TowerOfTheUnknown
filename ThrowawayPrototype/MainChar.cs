using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar : MonoBehaviour
{
    private int health = 1000;
    private new readonly string name = "Player";
    private int stamina = 100;
    /*public enum PlayerState
    {
        Selection,
        Battle,
        Dead
    }*/
 
    public int hp
    {
        get { return this.health; }
        set { this.health = value; }
    }
    public string PlayerNm
    {
        get { return name; }
    }
    public int sp
    {
        get { return this.stamina; }
        set { this.stamina = value; }
    }
}
