using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class GreedyAlgorithm
    {
        List<Dictionary<string, string>> trainingSet;
        public string[] headerRow;
        public string decisionHeader;
        public RuleSet ruleSet;

        public GreedyAlgorithm(List<Dictionary<string, string>> trainingSet,
                               string[] headerRow,
                               string decisionHeader)
        {
            this.trainingSet = trainingSet;
            this.headerRow = headerRow;
            this.decisionHeader = decisionHeader;
            ruleSet = new RuleSet(headerRow, decisionHeader);
            GetRules();
        }

        private void GetRules()
        {
            foreach(Dictionary<string,string> example in trainingSet)
            {
                Dictionary<string, string> rule = GetRule(example, trainingSet);
                string decisionValue = example[decisionHeader];
                rule.Add(decisionHeader, decisionValue);
                ruleSet.AddRule(rule);
            }
        }

        private Dictionary<string, string> GetRule(Dictionary<string, string> example, List<Dictionary<string, string>> trainingSet)
        {
            Dictionary<string, string> rule = new Dictionary<string, string> ();
            List<List<Dictionary<string, string>>> ListOfSubSets = new List<List<Dictionary<string, string>>>();
            List<SubsetInfo> subsetsInfo = new List<SubsetInfo>();
            for (int i = 0; i<headerRow.Length-1; i++)
            {
                ListOfSubSets.Add(GetSubsetOfAttribute(trainingSet, headerRow[i], example));
                subsetsInfo.Add(new SubsetInfo(i));
            }
            for(int i = 0; i<ListOfSubSets.Count(); i++)
            {
                subsetsInfo[i].uncertainty = getUncertaintyOfSubset(ListOfSubSets[i]);
            }
            int indexOfMinimalUncertainty = getIndexOfMinimalUncertainty(subsetsInfo);
            rule.Add(headerRow[indexOfMinimalUncertainty], example[headerRow[indexOfMinimalUncertainty]]);
            if(subsetsInfo[indexOfMinimalUncertainty].uncertainty == 0)
            {
                return rule;
            }
            else
            {
                return rule.Union(GetRule(example, ListOfSubSets[indexOfMinimalUncertainty])).ToDictionary(k => k.Key, v => v.Value);
            }
            
            
        }

        private int getIndexOfMinimalUncertainty(List<SubsetInfo> subsetsInfo)
        {
            int minimalIndex = 0;
            for(int i = 1; i < subsetsInfo.Count; i++)
            {
                if(subsetsInfo[i].uncertainty < subsetsInfo[minimalIndex].uncertainty)
                {
                    minimalIndex = i;
                }
            }
            return minimalIndex;
        }

        private int getUncertaintyOfSubset(List<Dictionary<string, string>> set)
        {
            // liczba par wierszy o różnych wartościach decyzyji
            int uncertainty = 0;
            for(int i = 0; i < set.Count-1; i++)
            {
                for(int j = i+1; j < set.Count; j++)
                {
                    if (set[i][decisionHeader] != set[j][decisionHeader])
                    {
                        uncertainty++;
                    }
                }
            }
            return uncertainty;
        }

        private List<Dictionary<string, string>> GetSubsetOfAttribute(List<Dictionary<string, string>> trainingSet, string attributeName, Dictionary<string, string> example)
        {
            List<Dictionary<string, string>> resultSet = new List<Dictionary<string, string>>();
            foreach(Dictionary<string,string> row in trainingSet)
            {
                if(row[attributeName] == example[attributeName])
                {
                    resultSet.Add(row);
                }
            }
            return resultSet;
        }

        private class SubsetInfo
        {
            public int uncertainty;
            public int headerIndex;
            public SubsetInfo(int headerIndex)
            {
                this.headerIndex = headerIndex;
            }
        }
    }
}
