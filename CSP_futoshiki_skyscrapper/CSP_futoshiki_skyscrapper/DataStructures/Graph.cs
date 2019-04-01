using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class Graph<T>
    {
        public int problemSize { get; set; }
        public virtual GraphNode<T>[,] nodes { get; set; }

        public Graph(int problemSize)
        {
            this.problemSize = problemSize;
            nodes = new GraphNode<T>[problemSize,problemSize];
        }

        public void AddNode(GraphNode<T> node)
        {
            nodes[node.xIndex, node.yIndex] = node;
        }

        public void AddEdgeFromSource(int srcX, int srcY, int destX, int destY, GraphEdge<T>.EDGE_TYPE_ENUM edgeType)
        {
            GraphNode<T> sourceNode = nodes[srcX, srcY];
            GraphNode<T> destinationNode = nodes[destX, destY];
            sourceNode.AddEdge(new GraphEdge<T>(sourceNode, destinationNode, edgeType));
        }

        public Graph<T> DeepClone()
        {
            Graph<T> futoshikiGraph = new Graph<T>(problemSize);
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    futoshikiGraph.nodes[i, j] = nodes[i, j].DeepClone();
                }
                for (int j = 0; j < problemSize; j++)
                {
                    for (int k = 0; k < problemSize; k++)
                    {
                        //skopiuję krawędź i podmienię jej source i dest

                    }
                }
            }
            return futoshikiGraph;
        }
    }
}
