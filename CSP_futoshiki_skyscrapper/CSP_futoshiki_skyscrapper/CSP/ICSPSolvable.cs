using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.CSP
{
    interface ICSPSolvable
    {
        ICSPSolvable DeepClone();
        void InitializeAllDomains();
        void AssignNewDataAndUpdateDomains(int xIndex, int yIndex, int newData);
        List<int> ReturnAllPossibilitiesForElement(CSPNode element);
        CSPNode ChooseTheMostLimitedAndNotSet();
        void PrintAllElements();
        bool IsSolved();
        bool IsAnyOfDomainsEmpty();
        void AssignNewData(int xIndex, int yIndex, int newData);
    }
}
