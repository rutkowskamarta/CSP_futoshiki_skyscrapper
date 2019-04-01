using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class GraphNode<T>
    {
        public T data { get; set; }
        public bool isMutable { get; }
        public int xIndex { get; }
        public int yIndex { get; }
        public List<GraphEdge<T>> outgoingEdges { get; } //krawędź wychodząca

        public GraphNode(T data, bool isMutable, int xIndex, int yIndex)
        {
            this.data = data;
            this.isMutable = isMutable;
            this.xIndex = xIndex;
            this.yIndex = yIndex;
            outgoingEdges = new List<GraphEdge<T>>();
        }

        public void AddEdge(GraphNode<T> other, GraphEdge<T>.EDGE_TYPE_ENUM edgeType)
        {
            outgoingEdges.Add(new GraphEdge<T>(this, other, edgeType));
        }

        public void AddEdge(GraphEdge<T> edge)
        {
            outgoingEdges.Add(edge);
        }

        public GraphNode<T> DeepClone()
        {
            GraphNode<T> graphNode = new GraphNode<T>(data, isMutable, xIndex, yIndex);
            return graphNode;
        }
       

        
    }
}
