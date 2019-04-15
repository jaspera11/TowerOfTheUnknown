using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace BT
//{
    public class RandomSelector : Node
    {
        protected List<Node> m_nodes = new List<Node>();
        private Queue<Node> m_nodes_queue = new Queue<Node>();
        private int m_n;
        private int[] m_weights;

        /** The constructor requires a lsit of child nodes to be  
         * passed in*/
        public RandomSelector(int n, List<Node> nodes, int[] weight)
        {
            m_nodes = nodes;
            m_n = n;
            m_weights = weight;
        }

        /* If any of the children reports a success, the selector will 
         * immediately report a success upwards. If all children fail, 
         * it will report a failure instead.*/
        public override NodeStates Evaluate()
        {
            Queue<Node> unused = new Queue<Node>();
            foreach(Node node in m_nodes)
            {
                unused.Enqueue(node);
            }

            while(unused.Peek() != null)
            {
                
                for (int i = 0; i < m_n; i++)
                {
                    int rand = Random.Range(0, 101);
                    if (rand < m_weights[i] && !m_nodes_queue.Contains(m_nodes[i]))
                    {
                        m_nodes_queue.Enqueue(m_nodes[i]);
                        unused.Dequeue();
                    }
                }
            }

            while(m_nodes_queue.Peek() != null)
            {
                
                switch (m_nodes_queue.Dequeue().Evaluate())
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

