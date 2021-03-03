using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    abstract class TrainMethod :ValidationMetod
    {
        abstract protected void Train();
        abstract protected void Test();
        abstract public void ExportSet(int set);
        public void ExportRules()
        {
            ExportCSV.ExportRuleSet(ruleSet);
        }

    }
}
