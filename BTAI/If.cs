using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BT
{
    public class If : Node
    {
        private Func<bool> m_f;
        protected List<Node> m_nodes = new List<Node>();

        public If(Func<bool> f, List<Node> nodes)
        {
            m_f = f;
            m_nodes = nodes;
        }

        public override NodeStates Evaluate()
        {
            if (m_f())
            {
                foreach (Node node in m_nodes)
                {
                    switch (node.Evaluate())
                    {
                        case NodeStates.FAILURE:
                            continue;
                        case NodeStates.SUCCESS:
                            m_nodeState = NodeStates.SUCCESS;
                            return m_nodeState;
                        case NodeStates.RUNNING:
                            m_nodeState = NodeStates.RUNNING;
                            return m_nodeState;
                        default:
                            continue;
                    }
                }
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            }
            else
            {
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            }
        }
    }
}
