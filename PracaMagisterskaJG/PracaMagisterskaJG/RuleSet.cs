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

    }
}