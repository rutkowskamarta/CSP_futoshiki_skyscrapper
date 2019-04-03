using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class Graph
    {
        public int problemSize { get; set; }
        public virtual GraphNode[,] nodes { get; set; }

        public Graph(int problemSize)
        {
            this.problemSize = problemSize;
            nodes = new GraphNode[problemSize,problemSize];
        }

        public void AddNode(GraphNode node)
        {
            nodes[node.xIndex, node.yIndex] = node;
        }

        public void AddEdgeFromSource(int srcX, int srcY, int destX, int destY, GraphEdge.EDGE_TYPE_ENUM edgeType)
        {
            GraphNode sourceNode = nodes[srcX, srcY];
            GraphNode destinationNode = nodes[destX, destY];
            sourceNode.AddEdge(new GraphEdge(sourceNode, destinationNode, edgeType));
        }
       
    }
}
