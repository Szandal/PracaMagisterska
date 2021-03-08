using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class TrainValidateTest : TrainMethod
    {
        public DataSetTVT dataSet;
        private RuleSet trainRuleSet;
       
        public TrainValidateTest(DataSetTVT dataSet)
        {
            this.dataSet = dataSet;
            Train();
            Validate();
            Test();
        }
        override
        protected void Train()
        {
            GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm(dataSet.trainingSet, dataSet.headerRow, dataSet.decisionHeader);
            trainRuleSet = greedyAlgorithm.ruleSet;
        }
        public void Validate()
        {
            ruleSet = Pruning();
        }
        override
        protected void Test()
        {
            quality = Math.Round(ruleSet.GetNumberOfWrongClassyfy(dataSet.testSet) / (double)dataSet.testSet.Count, 3);
        }
        private RuleSet Pruning()
        {
            int inaccuracyOfTrainingTable = GreedyAlgorithm.getUncertaintyOfSubset(dataSet.trainingSet, dataSet.decisionHeader);
            List<double> inaccuracyList = GetInaccuracyList(trainRuleSet.GetRuleSet(), inaccuracyOfTrainingTable);
            
            List<RuleSet> ruleSetsList = new List<RuleSet>();
            foreach(var i in inaccuracyList)
            {
                ruleSetsList.Add(new RuleSet(dataSet.headerRow, dataSet.decisionHeader));
            }
            for(int k = 0; k < ruleSetsList.Count; k++)
            {
                foreach(Rule rule in trainRuleSet.GetRuleSet())
                {
                    ruleSetsList[k].AddRule(GetSubruleOfInacurrany(rule.Copy(), inaccuracyList[k], inaccuracyOfTrainingTable));
                }
            } 
            return GetBestRuleSet(ruleSetsList);
        }

        private RuleSet GetBestRuleSet(List<RuleSet> ruleSetsList)
        {
            List<int> numberOfWrongClassyfyList = new List<int>();
            foreach(RuleSet ruleSet in ruleSetsList)
            {
                numberOfWrongClassyfyList.Add((int)ruleSet.GetNumberOfWrongClassyfy(dataSet.validationSet));
            }
            int indexOfLowest = 0;
            for(int i = 1; i < numberOfWrongClassyfyList.Count; i++)
            {
                if (numberOfWrongClassyfyList[indexOfLowest] > numberOfWrongClassyfyList[i])
                {
                    indexOfLowest = numberOfWrongClassyfyList[i];
                }
            }
            return ruleSetsList[indexOfLowest];
        }

        private Rule GetSubruleOfInacurrany(Rule rule, double inaccurancy, int inaccuracyOfTrainingTable)
        {
            List<Rule> subrulesList = new List<Rule>();
            do
            {
                rule.decisionValue = GetMostCommonDecision(GetSubsetOfRule(rule, dataSet.trainingSet));
                subrulesList.Add(rule.Copy());
                rule.DeleteFirstCondition();
            } while (rule.conditions.Count > 0);
            List<double> inaccurancyList = new List<double>();
            foreach(Rule subrule in subrulesList)
            {
                List<Dictionary<string, string>> subsetOfSubRule = GetSubsetOfRule(subrule.Copy(), dataSet.trainingSet);
                inaccurancyList.Add(GreedyAlgorithm.getUncertaintyOfSubset(subsetOfSubRule, dataSet.decisionHeader) / inaccuracyOfTrainingTable);
            }
            int k = 0;
            for(int i = 1; i < inaccurancyList.Count; i++)
            {
                if (inaccurancyList[i] != inaccurancyList[k] && inaccurancyList[i] <= inaccurancy)
                    k = i;
            }
            return subrulesList[k];
        }

      

       
        private List<double> GetInaccuracyList(List<Rule> rules, int inaccuracyOfTrainingTable)
        {
            List<double> inaccuracyList = new List<double>();
            foreach (Rule rule in rules)
            {
                Rule subrule = rule.Copy();
                do
                {
                    List<Dictionary<string, string>> subsetOfSubRule = GetSubsetOfRule(subrule, dataSet.trainingSet);
                    double inaccuracy = GreedyAlgorithm.getUncertaintyOfSubset(subsetOfSubRule, dataSet.decisionHeader) / inaccuracyOfTrainingTable;
                    if (!inaccuracyList.Contains(inaccuracy))
                        inaccuracyList.Add(inaccuracy);
                    subrule.DeleteFirstCondition();
                } while (subrule.conditions.Count>0);
            }
            inaccuracyList.Sort();
            return inaccuracyList;
        }

        private string GetMostCommonDecision(List<Dictionary<string, string>> subsetOfRule)
        {
            Dictionary<string, int> decisions = new Dictionary<string, int>();
            foreach(var rule in subsetOfRule)
            {
                if (decisions.ContainsKey(rule[dataSet.decisionHeader]))
                {
                    decisions[rule[dataSet.decisionHeader]]++;
                }
                else
                {
                    decisions.Add(rule[dataSet.decisionHeader], 1);
                }
            }
            int temp = 0;
            string result = "";
            foreach(var key in decisions.Keys)
            {
                if (decisions[key] > temp)
                {
                    temp = decisions[key];
                    result = key;
                }
            }
            return result;
        }

        private List<Dictionary<string, string>> GetSubsetOfRule(Rule rule, List<Dictionary<string, string>> set)
        {
            
            List<Dictionary<string, string>> subset = new List<Dictionary<string, string>>();
            foreach(Dictionary<string,string> example in set)
            {
                if(rule.CompareToExample(example))
                {
                    subset.Add(example);
                }
            }
            return subset;
        }

        override
        public void ExportSet(int set)
        {
            List<Dictionary<string, string>> setToExport;
            switch (set)
            {
                case 0:
                    setToExport = dataSet.trainingSet;
                    break;
                case 1:
                    setToExport = dataSet.validationSet;
                    break;
                case 2:
                    setToExport = dataSet.testSet;
                    break;
                default:
                    return;
            }
            ExportCSV.SaveSetToFile(setToExport, dataSet.headerRow);
        }

        
    }
}
