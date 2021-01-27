using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    public class Rule
    {
        public List<Dictionary<string, string>> conditions;
        public string decisionAttributte, decisionValue;
        public bool isMinimal = true;
        public Rule()
        {
            conditions = new List<Dictionary<string, string>>();
        }

        public void AddCondition(string attributte, string value)
        {
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add(attributte, value);
            conditions.Add(condition);
            CheckIsMinimal();
        }

       

        public void DeleteFirstCondition()
        {
            if (!isMinimal)
            {
                conditions.RemoveAt(0);
                CheckIsMinimal();
            }

        }

        public void DeleteLastCondition()
        {
            if (!isMinimal)
            {
                conditions.RemoveAt(conditions.Count-1);
                CheckIsMinimal();
            }
        }


        private void CheckIsMinimal()
        {
            isMinimal = conditions.Count <= 1;
        }

        public bool CompareToExample(Dictionary<string, string> example)
        {
            foreach(var condition in conditions)
            {
                if(condition.First().Value != example[condition.First().Key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
