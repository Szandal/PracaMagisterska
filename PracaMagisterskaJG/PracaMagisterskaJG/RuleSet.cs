using System.Collections.Generic;

namespace PracaMagisterskaJG
{
    public class RuleSet
    {
        private List<Dictionary<string, string>> ruleSet = new List<Dictionary<string, string>>();
        public string decisionHeader;
        public string[] headerRow;

        public RuleSet(string[] headerRow, string decisionHeader)
        {
            this.headerRow = headerRow;
            this.decisionHeader = decisionHeader;
        }

        public List<Dictionary<string, string>> GetRuleSet()
        {
            return ruleSet;
        }

        public void AddRule(Dictionary<string, string> rule)
        {
            ruleSet.Add(rule);
        }

    }
}