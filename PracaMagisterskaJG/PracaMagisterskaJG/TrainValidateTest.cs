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


            return ruleSet;
        }

        public void Test()
        {
            throw new NotImplementedException();
        }

    }
}
