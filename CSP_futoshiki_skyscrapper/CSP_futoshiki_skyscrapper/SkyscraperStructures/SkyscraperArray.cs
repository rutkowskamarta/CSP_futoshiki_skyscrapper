using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Linq;
using CSP_futoshiki_skyscrapper.CSP;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;

namespace CSP_futoshiki_skyscrapper.SkyscraperStructures
{
    class SkyscraperArray : ICSPSolvable
    {
        private delegate CSPNode choosingVariableHeuristicsDelegate();
        private choosingVariableHeuristicsDelegate choosingVariableHeuristicsMethod;

        public int arraySize { get; set; }
        public SkyscraperNode[,] nodes { get; set; }

        public SkyscraperArray(int arraySize)
        {
            this.arraySize = arraySize;
            nodes = new SkyscraperNode[arraySize, arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                    nodes[j, i] = new SkyscraperNode(0, j, i);
            }

            if(HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.GREEDY)
                choosingVariableHeuristicsMethod = ChooseFirstNotSet;

            else if(HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.MOST_LIMITED)
                choosingVariableHeuristicsMethod = ChooseTheMostLimitedAndNotSet;

            else if(HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.SMALL_DOMAIN)
                choosingVariableHeuristicsMethod = ChooseTheSmallestDomainAndGreatestConstraints;

        }

        public ICSPSolvable DeepClone()
        {
            SkyscraperArray skyscraperArray = new SkyscraperArray(arraySize);
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                    skyscraperArray.nodes[j, i] = (SkyscraperNode)nodes[j,i].DeepClone();
            }
            return skyscraperArray;
        }

        public void AssignNewData(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
        }

        #region FORWARD_CHECKING_METHODS

        public void InitializeAllDomains()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                    nodes[i, j].InitializeDomain(arraySize);
            }
        }

        public void AssignNewDataAndUpdateDomains(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
            UpdateAllDomains(nodes[xIndex, yIndex], newData);
        }

        private void UpdateAllDomains(SkyscraperNode node, int newData)
        {
            UpdateDomainInRowsAndColumns(node, newData);
        }

        private void UpdateDomainInRowsAndColumns(SkyscraperNode node, int data)
        {
            for (int i = 0; i < arraySize; i++)
            {
                if (i != node.yIndex)
                {
                    var columnElement = nodes[node.xIndex, i];
                    if(columnElement.data==0)
                        columnElement.domain.Remove(data);
                }

                if (i != node.xIndex)
                {
                    var rowElement = nodes[i, node.yIndex];
                    if(rowElement.data==0)
                        rowElement.domain.Remove(data);
                }
            }
        }
               
        public bool IsAnyOfDomainsEmpty()
        {
            return nodes.OfType<SkyscraperNode>().Any(i => i.domain.Count == 0);
        }
        #endregion

        #region BACKTRACKING_METHODS

        public List<int> ReturnAllPossibilitiesForElement(CSPNode element)
        {
            List<int> allPossibilities = InitializeAllPossibilities();
            List<int> itemsToRemove = RepeatedPossibilitiesInColumnOrRowOfElement((SkyscraperNode)element, allPossibilities);
            RemoveProperItemsFromAllPossibilities(allPossibilities, itemsToRemove);

            var column = nodes.OfType<SkyscraperNode>().Where(i => i.xIndex == element.xIndex);
            if (column.Count(i => i.data != 0) == arraySize)
            {
                itemsToRemove = PossibilitiesNotFulfillingConstraintsColumn((SkyscraperNode)element, allPossibilities, column);
                RemoveProperItemsFromAllPossibilities(allPossibilities, itemsToRemove);
            }
            
            var row = nodes.OfType<SkyscraperNode>().Where(i => i.yIndex == element.yIndex);
            if (row.Count(i => i.data != 0) == arraySize)
            {
                itemsToRemove = PossibilitiesNotFulfillingConstraintsRow((SkyscraperNode)element, allPossibilities, row);
                RemoveProperItemsFromAllPossibilities(allPossibilities, itemsToRemove);
            }

            return allPossibilities;
        }

        private List<int> InitializeAllPossibilities()
        {
            List<int> allPosibilites = new List<int>();
            for (int i = 0; i < arraySize; i++)
                allPosibilites.Add(i + 1);
            return allPosibilites;
        }

        private List<int> RepeatedPossibilitiesInColumnOrRowOfElement(SkyscraperNode node, List<int> allPosibilites)
        {
            List<int> itemsToRemove = new List<int>();

            for (int i = 0; i < arraySize; i++)
            {
                var columnElement = nodes[node.xIndex, i];
                if (allPosibilites.Contains(columnElement.data))
                    itemsToRemove.Add(columnElement.data);

                var rowElement = nodes[i, node.yIndex];
                if (allPosibilites.Contains(rowElement.data))
                    itemsToRemove.Add(rowElement.data);
            }

            return itemsToRemove;
        }


        private void RemoveProperItemsFromAllPossibilities(List<int> allPosibilites, List<int> itemsToRemove)
        {
            for (int i = 0; i < itemsToRemove.Count; i++)
                allPosibilites.Remove(itemsToRemove[i]);
        }

        private List<int> PossibilitiesNotFulfillingConstraintsColumn(SkyscraperNode element, List<int> allPossibilities,  IEnumerable<SkyscraperNode> column)
        {
            List<int> itemsToRemove = new List<int>();
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                if (!AreAllColumnConstraintsSatisfied(element, allPossibilities[i], column))
                    itemsToRemove.Add(element.data);
            }
            return itemsToRemove;
        }

        private List<int> PossibilitiesNotFulfillingConstraintsRow(SkyscraperNode element, List<int> allPossibilities, IEnumerable<SkyscraperNode> row)
        {
            List<int> itemsToRemove = new List<int>();
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                if (!AreAllRowConstraintsSatisfied(element, allPossibilities[i], row))
                    itemsToRemove.Add(element.data);
            }
            return itemsToRemove;
        }
        #endregion

        #region CHECKING_CONSTRAINTS

        public bool IsSolved()
        {
            int howManyDownside = 0;
            int howManyUpside = 0;
            int howManyLeft = 0;
            int howManyRight = 0;

            for (int i = 0; i < arraySize; i++)
            {
                var column = nodes.OfType<SkyscraperNode>().Where(item => item.xIndex == i);
                var row = nodes.OfType<SkyscraperNode>().Where(item => item.yIndex == i);

                howManyDownside = HowManySkyscrapersIsee(column.OrderByDescending(item => item.yIndex));
                if (SkyscraperProblemSingleton.lowerContraints[i] != 0 && howManyDownside != SkyscraperProblemSingleton.lowerContraints[i])
                    return false;
                howManyUpside = HowManySkyscrapersIsee(column);
                if (SkyscraperProblemSingleton.upperContraints[i] != 0 && howManyUpside != SkyscraperProblemSingleton.upperContraints[i])
                    return false;
                howManyLeft = HowManySkyscrapersIsee(row);
                if (SkyscraperProblemSingleton.leftContraints[i] != 0 && howManyLeft != SkyscraperProblemSingleton.leftContraints[i])
                    return false;
                howManyRight = HowManySkyscrapersIsee(row.OrderByDescending(item => item.xIndex));
                if (SkyscraperProblemSingleton.rightContraints[i] != 0 && howManyRight != SkyscraperProblemSingleton.rightContraints[i])
                    return false;
            }
            return true;
        }


        private bool AreAllColumnConstraintsSatisfied(SkyscraperNode element, int data, IEnumerable<SkyscraperNode> column)
        {
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_UPSIDE, column))
                return false;
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_DOWNSIDE, column))
                return false;

            return true;   
        }

        private bool AreAllRowConstraintsSatisfied(SkyscraperNode element, int data, IEnumerable<SkyscraperNode> row)
        {
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_LEFT, row))
                return false;
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_RIGHT, row))
                return false;
           
            return true;
        }

        private bool CheckConstraint(SkyscraperNode element, int data, SkyscraperProblemSingleton.CONSTRAINT_ENUM constraintType, IEnumerable<SkyscraperNode> rowOrColumn)
        {
            if(constraintType == SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_LEFT)
            {
                int constraint = SkyscraperProblemSingleton.leftContraints[element.yIndex];

                if (constraint == 0)
                    return true;
                else
                {
                    var rowOrColumnElement = rowOrColumn.ElementAt(element.xIndex);
                    rowOrColumnElement.data = data;
                    int howManyISee = HowManySkyscrapersIsee(rowOrColumn);
                    rowOrColumnElement.data = 0;
                    return howManyISee == constraint;
                }
            }
            else if (constraintType == SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_RIGHT)
            {
                int constraint = SkyscraperProblemSingleton.rightContraints[element.yIndex];

                if (constraint == 0)
                    return true;
                else
                {
                    var rowOrColumnElement = rowOrColumn.ElementAt(element.xIndex);
                    rowOrColumnElement.data = data;
                    var row = rowOrColumn.OrderByDescending(i => i.xIndex);
                    int howManyISee = HowManySkyscrapersIsee(row);
                    rowOrColumnElement.data = 0;
                    return howManyISee == constraint;
                }
            }
            else if (constraintType == SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_UPSIDE)
            {
                int constraint = SkyscraperProblemSingleton.upperContraints[element.xIndex];
                if (constraint == 0)
                    return true;
                else
                {
                    var rowOrColumnElement = rowOrColumn.ElementAt(element.yIndex);
                    rowOrColumnElement.data = data;
                    int howManyISee = HowManySkyscrapersIsee(rowOrColumn);
                    rowOrColumnElement.data = 0;
                    return howManyISee == constraint;
                }
            }
            else
            {
                int constraint = SkyscraperProblemSingleton.lowerContraints[element.xIndex];
                if (constraint == 0)
                    return true;
                else
                {
                    var rowOrColumnElement = rowOrColumn.ElementAt(element.yIndex);
                    rowOrColumnElement.data = data;
                    var column = rowOrColumn.OrderByDescending(i => i.yIndex);
                    int howManyISee = HowManySkyscrapersIsee(column);
                    rowOrColumnElement.data = 0;

                    return howManyISee == constraint;
                }
            }
        }

        private int HowManySkyscrapersIsee(IEnumerable<SkyscraperNode> rowOrColumn)
        {
            int howManyISee = 0;

            int max = rowOrColumn.First().data;
            for (int i = 0; i < rowOrColumn.Count(); i++)
            {
                var element = rowOrColumn.ElementAt(i);
                if (element.data >= max && element.data != 0)
                {
                    max = element.data;
                    howManyISee++;
                }
            }
            return howManyISee;
        }
        #endregion

        #region HEURISTICS
        public CSPNode ChooseElementByHeuristics()
        {
            return choosingVariableHeuristicsMethod();
        }

        private CSPNode ChooseTheMostLimitedAndNotSet()
        {
            var nodesCalculated = nodes.OfType<SkyscraperNode>().Where(i => i.data == 0);
            foreach (var n in nodesCalculated)
                n.measure = CalculateMeasure(n);
            var ordered = nodesCalculated.OrderByDescending(i => i.measure);
            if (ordered.Count() == 0)
                return null;
            else
                return ordered.First();
        }

        private int CalculateMeasure(SkyscraperNode node)
        {
            int rowMeasure = nodes.OfType<SkyscraperNode>().Where(i => i.yIndex == node.yIndex && i.data != 0).Count();
            int columnMeasure = nodes.OfType<SkyscraperNode>().Where(i => i.xIndex == node.xIndex && i.data != 0).Count();
            int constraintMeasure = 0;
            if (SkyscraperProblemSingleton.leftContraints[node.yIndex] != 0)
                constraintMeasure++;
            if (SkyscraperProblemSingleton.rightContraints[node.yIndex] != 0)
                constraintMeasure++;
            if (SkyscraperProblemSingleton.upperContraints[node.xIndex] != 0)
                constraintMeasure++;
            if (SkyscraperProblemSingleton.lowerContraints[node.xIndex] != 0)
                constraintMeasure++;
            return rowMeasure + columnMeasure + constraintMeasure;
        }

        private CSPNode ChooseFirstNotSet()
        {
            var nodesList = nodes.OfType<SkyscraperNode>().Where(i => i.data == 0);
            if (nodesList.Count() == 0)
                return null;
            else
                return nodesList.First();

        }

        private CSPNode ChooseTheSmallestDomainAndGreatestConstraints()
        {
            List<SkyscraperNode> ordered;
            if (ALGORITHM_TYPE == ALGORITHM_TYPE_ENUM.FORWARD_CHECKING)
                ordered = nodes.OfType<SkyscraperNode>().Where(item => item.data == 0).OrderBy(i => i.domain.Count).ToList();
            else
                ordered = nodes.OfType<SkyscraperNode>().Where(item => item.data == 0).OrderBy(item => ReturnAllPossibilitiesForElement(item).Count).ToList();
            if (ordered.Count == 0)
                return null;
            return ordered.First();
        }


        #endregion

        #region PRINTING

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("  ");
            for (int i = 0; i < arraySize; i++)
                stringBuilder.Append(SkyscraperProblemSingleton.upperContraints[i] + ";");
            stringBuilder.Append("\n");
            stringBuilder.Append("  ");
            for (int i = 0; i < arraySize; i++)
                stringBuilder.Append("--");
            stringBuilder.Append("\n");
            for (int i = 0; i < arraySize; i++)
            {
                stringBuilder.Append(SkyscraperProblemSingleton.leftContraints[i] + "|");
                for (int j = 0; j < arraySize; j++)
                    stringBuilder.Append(nodes[j, i].data + ";");
                stringBuilder.Append("|" + SkyscraperProblemSingleton.rightContraints[i]);
                stringBuilder.Append("\n");
            }
            stringBuilder.Append("  ");
            for (int i = 0; i < arraySize; i++)
                stringBuilder.Append("--");
            stringBuilder.Append("\n");
            stringBuilder.Append("  ");
            for (int i = 0; i < arraySize; i++)
                stringBuilder.Append(SkyscraperProblemSingleton.lowerContraints[i] + ";");
            stringBuilder.Append("\n");
            stringBuilder.Append("\n");
            return stringBuilder.ToString();
        }

        public void PrintAllElements()
        {
            WriteLine(ToString());
        }
        #endregion
    }
}
