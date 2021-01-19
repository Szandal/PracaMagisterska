using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class TrainValidateTest
    {
        private DataSet dataSet;
        private RuleSet trainRuleSet, validateRuleSet;

        public TrainValidateTest(DataSet dataSet)
        {
            this.dataSet = dataSet;
        }
        
        public void Train()
        {
            GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm(dataSet.trainingSet, dataSet.headerRow, dataSet.decisionHeader);
            trainRuleSet = greedyAlgorithm.ruleSet;
        }
        public void Validate()
        {
            validateRuleSet = Pruning();
        }

        private RuleSet Pruning()
        {
            RuleSet ruleSet = new RuleSet(dataSet.headerRow, dataSet.decisionHeader);
            RuleSet subruleSet = new RuleSet(dataSet.headerRow, dataSet.decisionHeader);
            int inaccuracyOfTrainingTable = GreedyAlgorithm.getUncertaintyOfSubset(dataSet.trainingSet, dataSet.decisionHeader);
            List<double> inaccuracyList = new List<double>();
            foreach(Dictionary<string,string> rule in trainRuleSet.GetRuleSet())
            {
                List<Dictionary<string, string>> subsetOfRule = GetSubsetOfRule(rule, dataSet.trainingSet);
                Dictionary<string, string> subrule = rule;
                subrule.Remove(dataSet.decisionHeader);
                subrule.Add(dataSet.decisionHeader, GetMostCommonDecision(subsetOfRule));
                subruleSet.AddRule(subrule);
                double inaccuracy = GreedyAlgorithm.getUncertaintyOfSubset(subsetOfRule, dataSet.decisionHeader) / inaccuracyOfTrainingTable;
                if(!inaccuracyList.Contains(inaccuracy))
                    inaccuracyList.Add(inaccuracy);
            }
            inaccuracyList.Sort();
            List<RuleSet> ruleSetsList = new List<RuleSet>();
            foreach(double i in inaccuracyList)
            {
                ruleSetsList.Add(new RuleSet(dataSet.headerRow, dataSet.decisionHeader));
            }

            for(int i=0; i<inaccuracyList.Count; i++)
            {

            }
            return ruleSet;
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

        private List<Dictionary<string, string>> GetSubsetOfRule(Dictionary<string, string> rule, List<Dictionary<string, string>> set)
        {
            rule.Remove(dataSet.decisionHeader);
            List<Dictionary<string, string>> subset = new List<Dictionary<string, string>>();
            foreach(Dictionary<string,string> example in set)
            {
                if(CompareRuleAndExample(rule, example))
                {
                    subset.Add(example);
                }
            }
            return subset;
        }

        private bool CompareRuleAndExample(Dictionary<string, string> rule, Dictionary<string, string> example)
        {
            foreach (string attributte in rule.Keys)
            {
                if (rule[attributte] != example[attributte])
                {
                    return false;
                }
            }
            return true;
        }

        public void Test()
        {
            throw new NotImplementedException();
        }

    }
}
