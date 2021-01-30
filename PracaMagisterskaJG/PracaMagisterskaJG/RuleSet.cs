using System;
using System.Collections.Generic;

namespace PracaMagisterskaJG
{
    public class RuleSet
    {

        private List<Rule> ruleSet = new List<Rule>();
        public string decisionHeader;
        public string[] headerRow;

        public RuleSet(string[] headerRow, string decisionHeader)
        {
            this.headerRow = headerRow;
            this.decisionHeader = decisionHeader;
        }

        public List<Rule> GetRuleSet()
        {
            return ruleSet;
        }

        public void AddRule(Rule rule)
        {
            ruleSet.Add(rule);
        }

        public string GetListClassyfy(Dictionary<string,string> example)
        {
            foreach(Rule rule in ruleSet)
            {
                if (rule.CompareToExample(example))
                {
                    return rule.decisionValue;
                }
            }
            return GetMostCommonDecision();
        }

        private string GetMostCommonDecision()
        {
            Dictionary<string, int> decisions = new Dictionary<string, int>();
            foreach(Rule rule in ruleSet)
            {
                if (decisions.ContainsKey(rule.decisionValue))
                {
                    decisions[rule.decisionValue]++;
                }
                else
                {
                    decisions.Add(rule.decisionValue, 1);
                }
            }
            string mostCommon = "";
            int number = 0;
            foreach(var decision in decisions)
            {
                if (decision.Value > number)
                {
                    mostCommon = decision.Key;
                }
            }
            return mostCommon;
        }
        public string PrintRules()
        {
            string rulesToPrint = "";
            foreach (Rule rule in ruleSet)
            {
                rulesToPrint += rule.PrintRule() + '\n';
            }
            return rulesToPrint;
        }
    }
}