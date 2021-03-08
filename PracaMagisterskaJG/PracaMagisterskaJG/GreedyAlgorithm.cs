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
                if (example == null)
                    continue;
                Rule rule = new Rule();
                rule.decisionAttributte = decisionHeader; 
                rule.decisionValue = example[decisionHeader];
                rule.conditions = GetRule(example, trainingSet);
                ruleSet.AddRule(rule);
            }
        }

        private List<Dictionary<string, string>> GetRule(Dictionary<string, string> example, List<Dictionary<string, string>> trainingSet)
        {
            List<Dictionary<string, string>> rule = new List<Dictionary<string, string>>();
            List<List<Dictionary<string, string>>> ListOfSubSets = new List<List<Dictionary<string, string>>>();
            List<SubsetInfo> subsetsInfo = new List<SubsetInfo>();
            for (int i = 0; i<headerRow.Length-1; i++)
            {
                ListOfSubSets.Add(GetSubsetOfAttribute(trainingSet, headerRow[i], example));
                subsetsInfo.Add(new SubsetInfo(i));
            }
            for(int i = 0; i<ListOfSubSets.Count(); i++)
            {
                subsetsInfo[i].uncertainty = getUncertaintyOfSubset(ListOfSubSets[i], decisionHeader);
            }
            int indexOfMinimalUncertainty = getIndexOfMinimalUncertainty(subsetsInfo);
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add(headerRow[indexOfMinimalUncertainty], example[headerRow[indexOfMinimalUncertainty]]);
            rule.Add(condition);
            if(subsetsInfo[indexOfMinimalUncertainty].uncertainty != 0)
            {
                List<Dictionary<string, string>> nextConditions = GetRule(example, ListOfSubSets[indexOfMinimalUncertainty]);
                foreach(var con in nextConditions)
                {
                    rule.Add(con);
                }
            }
            return rule;
            
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

        public static int getUncertaintyOfSubset(List<Dictionary<string, string>> set, string decisionHeader)
        {
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
                if (row == null)
                    continue;
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
