using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIDraft : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy Phase - Purely Pseudocode
        for (int i = skilln; i >= 0; i--)
        {
            if (Enemy.sp >= skill[1].reqSP)  //Assuming highest index is most damaging
            {
                Enemy.useSkill(1);
                attacked = true;
            }
        }
        if(!attacked)
        {
            Enemy.basicAttack();
        }
        
        
    }
}
