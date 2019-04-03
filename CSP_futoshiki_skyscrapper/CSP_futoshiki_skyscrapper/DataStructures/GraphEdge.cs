using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.DataStructures
{
    class GraphEdge
    {
        public enum EDGE_TYPE_ENUM { SOURCE_GREATER, DESTINATION_GRATER}

        public GraphNode sourceNode { get; set; }
        public GraphNode destinationNode { get; set; }
        public EDGE_TYPE_ENUM edgeType { get; set; }

        public GraphEdge(GraphNode sourceNode, GraphNode destinationNode, EDGE_TYPE_ENUM edgeType)
        {
            this.sourceNode = sourceNode;
            this.destinationNode = destinationNode;
            this.edgeType = edgeType;
        }

        public bool IsEdgeSatisfied()
        {
            if (edgeType == EDGE_TYPE_ENUM.DESTINATION_GRATER)
                return destinationNode.IsGreaterThan(sourceNode.data);
            else
                return sourceNode.IsGreaterThan(destinationNode.data);
        }

        public GraphEdge DeepClone()
        {
            return new GraphEdge(sourceNode, destinationNode, edgeType);
        }

        public override string ToString()
        {
            string edgeTypeString = "";
            if (edgeType == EDGE_TYPE_ENUM.DESTINATION_GRATER)
                edgeTypeString = "<";
            else
                edgeTypeString = ">";
            return $" src: ({sourceNode.xIndex},{sourceNode.yIndex}) {edgeTypeString} dst: ({destinationNode.xIndex},{destinationNode.yIndex}) ";
        }
    }
}
