using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace BT
//{
    public class Selector : Node
    {
        //Children nodes
        protected List<Node> m_nodes = new List<Node>();

        public Selector(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        /* If any of the children reports a success, the selector will 
         * immediately report a success upwards. If all children fail, 
         * it will report a failure instead.*/
        public override NodeStates Evaluate()
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
    }
//}