using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.CSP;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class GraphNode : CSPNode
    {
        
        public List<GraphEdge> outgoingEdges { get; set; } //krawędź wychodząca

        public GraphNode(int data, bool isMutable, int xIndex, int yIndex) : base(data, isMutable, xIndex, yIndex)
        {
            outgoingEdges = new List<GraphEdge>();
        }

        public void InitializeDomain(int problemSize)
        {
            for (int i = 0; i < problemSize; i++)
                domain.Add(i + 1);
        }
        
        public void AddEdge(GraphEdge edge)
        {
            outgoingEdges.Add(edge);
        }

        public override CSPNode DeepClone()
        {
            GraphNode graphNode = new GraphNode(data, isMutable, xIndex, yIndex);
            graphNode.outgoingEdges = new List<GraphEdge>();
            graphNode.measure = measure;
            graphNode.domain = new List<int>();
            foreach (var item in outgoingEdges)
                graphNode.outgoingEdges.Add(item.DeepClone());

            for (int i = 0; i < domain.Count; i++)
                graphNode.domain.Add(domain[i]);

            return graphNode;
        }
       
        public bool AreConstraintsSatisfied()
        {
            for (int i = 0; i < outgoingEdges.Count; i++)
            {
                if (!outgoingEdges[i].IsEdgeSatisfied())
                    return false;
            }
            return true;
        }

        public bool IsGreaterThan(int other)
        {
            return data>other;
        }
    }
}
