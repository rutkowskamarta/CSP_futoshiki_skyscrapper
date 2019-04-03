using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    //krawędź skierowana, ważona

    class GraphEdge<T>
    {
        public enum EDGE_TYPE_ENUM { SOURCE_GREATER, DESTINATION_GRATER}

        public GraphNode<T> sourceNode { get; set; }
        public GraphNode<T> destinationNode { get; set; }
        public EDGE_TYPE_ENUM edgeType { get; set; }

        public GraphEdge(GraphNode<T> sourceNode, GraphNode<T> destinationNode, EDGE_TYPE_ENUM edgeType)
        {
            this.sourceNode = sourceNode;
            this.destinationNode = destinationNode;
            this.edgeType = edgeType;
        }

        public override string ToString()
        {
            string edgeTypeString = "";
            if(edgeType == EDGE_TYPE_ENUM.DESTINATION_GRATER)
            {
                edgeTypeString = "<";
            }
            else
            {
                edgeTypeString = ">";
            }
            return $" src: ({sourceNode.xIndex},{sourceNode.yIndex}) {edgeTypeString} dst: ({destinationNode.xIndex},{destinationNode.yIndex}) ";
        }

        public GraphEdge<T> DeepClone()
        {
            return new GraphEdge<T>(sourceNode, destinationNode, edgeType);
        }

        public bool IsEdgeSatisfied()
        {
            if(edgeType == EDGE_TYPE_ENUM.DESTINATION_GRATER)
            {
                return sourceNode.CompareTo(destinationNode.data) < 0;
            }
            else
            {
                return destinationNode.CompareTo(sourceNode.data) < 0;
            }
        }

    }
}
