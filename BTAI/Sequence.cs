using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class Sequence : Node
    {
        //List of children
        private List<Node> m_nodes = new List<Node>();

       
        public Sequence(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        /* If any child node returns a failure, the entire node fails. Whence all  
         * nodes return a success, the node reports a success. */
        public override NodeStates Evaluate()
        {
            bool anyChildRunning = false;

            foreach (Node node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.FAILURE:
                        m_nodeState = NodeStates.FAILURE;
                        return m_nodeState;
                    case NodeStates.SUCCESS:
                        continue;
                    case NodeStates.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        m_nodeState = NodeStates.SUCCESS;
                        return m_nodeState;
                }
            }
            m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}