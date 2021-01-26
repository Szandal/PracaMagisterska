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
                    ruleSetsList[k].AddRule(GetSubruleOfInacurrany(rule, inaccuracyList[k], inaccuracyOfTrainingTable));
                }
            }
            ruleSet = GetBestRuleSet(ruleSetsList);
            return ruleSet;
        }

        private RuleSet GetBestRuleSet(List<RuleSet> ruleSetsList)
        {
            throw new NotImplementedException();
        }

        private Rule GetSubruleOfInacurrany(Rule rule, double inaccurancy, int inaccuracyOfTrainingTable)
        {
            List<Rule> subrulesList = new List<Rule>();
            do
            {
                rule.decisionValue = GetMostCommonDecision(GetSubsetOfRule(rule, dataSet.trainingSet));
                subrulesList.Add(rule);
                rule.DeleteFirstCondition();
            } while (rule.conditions.Count > 0);
            List<double> inaccurancyList = new List<double>();
            foreach(Rule subrule in subrulesList)
            {
                List<Dictionary<string, string>> subsetOfSubRule = GetSubsetOfRule(subrule, dataSet.trainingSet);
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
                Rule subrule = rule;
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
                if(CompareRuleAndExample(rule, example))
                {
                    subset.Add(example);
                }
            }
            return subset;
        }

        private bool CompareRuleAndExample(Rule rule, Dictionary<string, string> example)
        {

            foreach (var condition in rule.conditions)
            {
                if (condition.First().Value != example[condition.First().Key])
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
