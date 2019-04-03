using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class GraphNode<T> : IComparable<T>
    {
        public T data { get; set; }
        public bool isMutable { get; }
        public int xIndex { get; }
        public int yIndex { get; }
        public int measure { get; set; }
        public List<GraphEdge<T>> outgoingEdges { get; set; } //krawędź wychodząca

        //for forwardChecking
        public List<int> domain { get; set; }

        public GraphNode(T data, bool isMutable, int xIndex, int yIndex)
        {
            this.data = data;
            this.isMutable = isMutable;
            this.xIndex = xIndex;
            this.yIndex = yIndex;
            outgoingEdges = new List<GraphEdge<T>>();
            
            domain = new List<int>();
        }

        public void InitializeDomain(int problemSize)
        {
            for (int i = 0; i < problemSize; i++)
                domain.Add(i + 1);
        }
        
        public void AddEdge(GraphEdge<T> edge)
        {
            outgoingEdges.Add(edge);
        }

        public GraphNode<T> DeepClone()
        {
            GraphNode<T> graphNode = new GraphNode<T>(data, isMutable, xIndex, yIndex);
            graphNode.outgoingEdges = new List<GraphEdge<T>>();
            graphNode.measure = measure;
            domain = new List<int>();
            foreach (var item in outgoingEdges)
            {
                graphNode.outgoingEdges.Add(item.DeepClone());
            }
            for (int i = 0; i < domain.Count; i++)
            {
                graphNode.domain.Add(domain[i]);
            }
            return graphNode;
        }
       
        public bool AreConstraintsSatisfied()
        {
            for (int i = 0; i < outgoingEdges.Count; i++)
            {
                if (!outgoingEdges[i].IsEdgeSatisfied())
                {
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(T other)
        {
            return Comparer<T>.Default.Compare(data, other);
        }
    }
}
