using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace BT
//{
    public class Inverter : Node
    {
        //Child node
        private Node m_node;

        public Node node
        {
            get { return m_node; }
        }

        public Inverter(Node node)
        {
            m_node = node;
        }

        /* Reports a success if the child fails and 
         * a failure if the child succeeds. Running will report 
         * as running */
        public override NodeStates Evaluate()
        {
            switch (m_node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
            }
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
//}