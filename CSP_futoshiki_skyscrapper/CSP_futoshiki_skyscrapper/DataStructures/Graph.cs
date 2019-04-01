using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class Graph<T>
    {
        public GraphNode<T>[,] nodes { get; set; }

        public Graph(int problemSize)
        {
            nodes = new GraphNode<T>[problemSize,problemSize];
        }

        public void AddNode(GraphNode<T> node)
        {
            nodes[node.xIndex, node.yIndex] = node;
        }

    }
}
