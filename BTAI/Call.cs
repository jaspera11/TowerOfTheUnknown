using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//namespace BT
//{
    public class Call : Node
    {
        private Action m_f;

        public Call(Action f)
        {
            m_f = f;
        }

        public override NodeStates Evaluate()
        {
            m_f();

            return NodeStates.SUCCESS;
        }

    }
//}