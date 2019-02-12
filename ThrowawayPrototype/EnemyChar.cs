using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChar : MonoBehaviour
{
    private int health = 1000;
    private new readonly string name = "Enemy";
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public int hp
    {
        get { return health; }
        set { this.health = value; }
    }
    public string EnemyNm
    {
        get { return name; }
    }
    
}
