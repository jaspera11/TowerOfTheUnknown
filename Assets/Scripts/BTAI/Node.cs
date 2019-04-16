//Some code in tree inspired by tutorial at: https://hub.packtpub.com/building-your-own-basic-behavior-tree-tutorial/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace BT
//{
    [System.Serializable]
    public abstract class Node
    {
        public enum NodeStates
        {
            RUNNING,
            FAILURE,
            SUCCESS
        }
        /* Delegate that returns the state of the node.*/
        public delegate NodeStates NodeReturn();

        /* The current state of the node */
        protected NodeStates m_nodeState;

        public NodeStates nodeState
        {
            get { return m_nodeState; }
        }

        /* The constructor for the node */
        public Node() { }

        /* Implementing classes use this method to evaluate the desired set of conditions */
        public abstract NodeStates Evaluate();

    }
//}