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
            {
                choosingVariableHeuristicsMethod = ChooseFirstNotSet;
            }
            else if(HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.MOST_LIMITING)
            {
                choosingVariableHeuristicsMethod = ChooseTheMostLimitedAndNotSet;
            }
            else if(HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.SMALL_DOMAIN_AND_MANY_CONSTRAINTS)
            {
                choosingVariableHeuristicsMethod = ChooseTheSmallestDomainAndGreatestConstraints;
            }
            
        }

        public ICSPSolvable DeepClone()
        {
            SkyscraperArray skyscraperArray = new SkyscraperArray(arraySize);
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    skyscraperArray.nodes[j, i] = (SkyscraperNode)nodes[j,i].DeepClone();
                }
            }
            return skyscraperArray;
        }

        #region FORWARD_CHECKING_METHODS

        public void InitializeAllDomains()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    nodes[i, j].InitializeDomain(arraySize);
                   
                }
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
            return nodes.OfType<SkyscraperNode>().AsParallel().Any(i => i.domain.Count == 0);
        }
        #endregion

        public List<int> ReturnAllPossibilitiesForElement(CSPNode element)
        {
            List<int> allPossibilities = InitializeAllPossibilities();
            //te rzutowanie i powielające się funkcje lepiej napisać można, zamiast interfejsu, klasa?
            List<int> itemsToRemove = RepeatedPossibilitiesInColumnOrRowOfElement((SkyscraperNode)element, allPossibilities);
            RemoveProperItemsFromAllPossibilities(allPossibilities, itemsToRemove);

            //possibilities not fulfillng ma być abstract
            var column = nodes.OfType<SkyscraperNode>().Select(i => i).Where(i => i.xIndex == element.xIndex).ToList();
            if (column.Count(i => i.data != 0) == arraySize)
            {

                itemsToRemove = PossibilitiesNotFulfillingConstraintsColumn((SkyscraperNode)element, allPossibilities, column);
                RemoveProperItemsFromAllPossibilities(allPossibilities, itemsToRemove);
            }
            
            var row = nodes.OfType<SkyscraperNode>().Select(i => i).Where(i => i.yIndex == element.yIndex).ToList();
            if (row.Count(i => i.data != 0) == arraySize)
            {

                itemsToRemove = PossibilitiesNotFulfillingConstraintsRow((SkyscraperNode)element, allPossibilities, row);
                RemoveProperItemsFromAllPossibilities(allPossibilities, itemsToRemove);
            }

            return allPossibilities;
        }
        
        private List<int> PossibilitiesNotFulfillingConstraintsColumn(SkyscraperNode element, List<int> allPossibilities,  List<SkyscraperNode> column)
        {
            List<int> itemsToRemove = new List<int>();
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                if (!AreAllColumnConstraintsSatisfied(element, allPossibilities[i], column))
                {
                    itemsToRemove.Add(element.data);
                }
            }
            return itemsToRemove;

        }

        private List<int> PossibilitiesNotFulfillingConstraintsRow(SkyscraperNode element, List<int> allPossibilities, List<SkyscraperNode> row)
        {
            List<int> itemsToRemove = new List<int>();
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                if (!AreAllRowConstraintsSatisfied(element, allPossibilities[i], row))
                {
                    itemsToRemove.Add(element.data);
                }
            }
            return itemsToRemove;

        }

        private bool AreAllColumnConstraintsSatisfied(SkyscraperNode element, int data,List<SkyscraperNode> column)
        {
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_UPSIDE, column))
                return false;
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_DOWNSIDE, column))
                return false;

            return true;   
        }

        private bool AreAllRowConstraintsSatisfied(SkyscraperNode element, int data, List<SkyscraperNode> row)
        {
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_LEFT, row))
                return false;
            if (!CheckConstraint(element, data, SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_RIGHT, row))
                return false;
           

            return true;
        }

        private bool CheckConstraint(SkyscraperNode element, int data, SkyscraperProblemSingleton.CONSTRAINT_ENUM constraintType, List<SkyscraperNode> rowOrColumn)
        {
            if(constraintType == SkyscraperProblemSingleton.CONSTRAINT_ENUM.LOOK_FROM_LEFT)
            {
                int constraint = SkyscraperProblemSingleton.leftContraints[element.yIndex];

                if (constraint == 0)
                    return true;
                else
                {
                    rowOrColumn[element.xIndex].data = data;
                    int howManyISee = HowManySkyscrapersIsee(rowOrColumn);
                    rowOrColumn[element.xIndex].data = 0;
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
                    rowOrColumn[element.xIndex].data = data;
                    var row = rowOrColumn.AsParallel().OrderByDescending(i => i.xIndex).ToList();
                    int howManyISee = HowManySkyscrapersIsee(row);
                    rowOrColumn[element.xIndex].data = 0;
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
                    rowOrColumn[element.yIndex].data = data;
                    int howManyISee = HowManySkyscrapersIsee(rowOrColumn);
                    rowOrColumn[element.yIndex].data = 0;
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
                    rowOrColumn[element.yIndex].data = data;
                    var column = rowOrColumn.AsParallel().OrderByDescending(i => i.yIndex).ToList();
                    int howManyISee = HowManySkyscrapersIsee(column);
                    rowOrColumn[element.yIndex].data = 0;
                    
                    return howManyISee == constraint;
                }
            }
        }

        private int HowManySkyscrapersIsee(List<SkyscraperNode> rowOrColumn)
        {
            int howManyISee = 0;

            int max = rowOrColumn[0].data;
            for (int i = 0; i < rowOrColumn.Count; i++)
            {
                if (rowOrColumn[i].data >= max && rowOrColumn[i].data != 0)
                {
                    max = rowOrColumn[i].data;
                    howManyISee++;
                }
            }
            return howManyISee;
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

        public CSPNode ChooseElementByHeuristics()
        {
            return choosingVariableHeuristicsMethod();
        }

        private CSPNode ChooseTheMostLimitedAndNotSet()
        {
            nodes.OfType<SkyscraperNode>().Where(i => i.data == 0).ToList().ForEach(i => i.measure = CalculateMeasure(i));
            var ordered = nodes.OfType<SkyscraperNode>().Select(i => i).Where(i => i.data == 0).OrderByDescending(i => i.measure).ToList();
            if (ordered.Count == 0)
                return null;
            else
                return ordered.First();
        }

        private CSPNode ChooseFirstNotSet()
        {
            var nodesList = nodes.OfType<SkyscraperNode>().Where(i => i.data == 0).ToList();
            if (nodesList.Count == 0)
                return null;
            else
                return nodesList.First();

        }

        private CSPNode ChooseTheSmallestDomainAndGreatestConstraints()
        {

            var ordered = nodes.OfType<SkyscraperNode>().OrderBy(i => i.domain.Count).ToList();
            if (ordered.Count == 0)
                return null;
            var elementsWithSmallestDomain = ordered.Select(item => item).Where(item => item.domain == ordered[0].domain);
            return elementsWithSmallestDomain.First();

            //tutuutututu return ordered.First();
        }
        //tu dopisać drugą heurystykę, może greedy

        private int CalculateMeasure(SkyscraperNode node)
        {
            int rowMeasure = nodes.OfType<SkyscraperNode>().Select(i=>i).Where(i => i.yIndex == node.yIndex && i.data != 0).Count();
            int columnMeasure = nodes.OfType<SkyscraperNode>().Select(i => i).Where(i => i.xIndex == node.xIndex && i.data != 0).Count();
            int constraintMeasure = 0;
            if (SkyscraperProblemSingleton.leftContraints[node.yIndex] != 0)
                constraintMeasure++;
            if (SkyscraperProblemSingleton.rightContraints[node.yIndex] != 0)
                constraintMeasure++;
            if (SkyscraperProblemSingleton.upperContraints[node.xIndex] != 0)
                constraintMeasure++;
            if (SkyscraperProblemSingleton.lowerContraints[node.xIndex] != 0)
                constraintMeasure++;
            return rowMeasure + columnMeasure+constraintMeasure;
        }

        
        public bool IsSolved()
        {
            int howManyDownside = 0;
            int howManyUpside = 0;
            int howManyLeft = 0;
            int howManyRight = 0;

            for (int i = 0; i < arraySize; i++)
            {
                var column = nodes.OfType<SkyscraperNode>().Select(item => item).Where(item => item.xIndex == i).ToList();
                var row = nodes.OfType<SkyscraperNode>().Select(item => item).Where(item => item.yIndex == i).ToList();

                howManyDownside = HowManySkyscrapersIsee(column.OrderByDescending(item=>item.yIndex).ToList());
                if (SkyscraperProblemSingleton.lowerContraints[i] != 0 && howManyDownside != SkyscraperProblemSingleton.lowerContraints[i])
                    return false;

                howManyUpside = HowManySkyscrapersIsee(column);
                if (SkyscraperProblemSingleton.upperContraints[i] != 0 && howManyUpside != SkyscraperProblemSingleton.upperContraints[i] )
                    return false;
                howManyLeft = HowManySkyscrapersIsee(row);
                if (SkyscraperProblemSingleton.leftContraints[i] != 0 && howManyLeft != SkyscraperProblemSingleton.leftContraints[i] )
                    return false;
                howManyRight = HowManySkyscrapersIsee(row.OrderByDescending(item => item.xIndex).ToList());
                if (SkyscraperProblemSingleton.rightContraints[i] != 0 && howManyRight != SkyscraperProblemSingleton.rightContraints[i])
                    return false;
                
            }

            return true;
        }

        public void AssignNewData(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
        }

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

        #region PRINTING
        

        public void PrintAllElements()
        {
            WriteLine(ToString());
        }
        #endregion
    }
}
