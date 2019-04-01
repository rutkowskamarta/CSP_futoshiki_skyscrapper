using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    //krawędź skierowana, ważona

    class GraphEdge<T>
    {
        public enum EDGE_TYPE_ENUM { SOURCE_GREATER, DESTINATION_GRATER, NO_TYPE }

        public GraphNode<T> sourceNode { get; set; }
        public GraphNode<T> destinationNode { get; set; }
        public EDGE_TYPE_ENUM edgeType { get; set; }

        public GraphEdge(GraphNode<T> sourceNode, GraphNode<T> destinationNode, EDGE_TYPE_ENUM edgeType)
        {
            this.sourceNode = sourceNode;
            this.destinationNode = destinationNode;
            this.edgeType = edgeType;
        }

        public GraphEdge(EDGE_TYPE_ENUM edgeType)
        {
            sourceNode = null;
            destinationNode = null;
            this.edgeType = edgeType;
        }
    }
}
