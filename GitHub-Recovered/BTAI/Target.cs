using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//namespace BT
//{
public class Target : Node
{
    private Action<UnitStats> m_f;
    private UnitStats m_target;

    public Target(Action<UnitStats> f, UnitStats target)
    {
        m_f = f;
        m_target = target;
    }

    public override NodeStates Evaluate()
    {
        m_f(m_target);

        return NodeStates.SUCCESS;
    }

}
//}