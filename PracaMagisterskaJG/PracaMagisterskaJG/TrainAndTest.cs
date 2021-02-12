using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class TrainAndTest
    {
        public DataSetTT dataSet;
        public RuleSet ruleSet;
        public int quality;
        public TrainAndTest(DataSetTT dataSet)
        {
            this.dataSet = dataSet;
            Train();
            Test();
        }

        private void Test()
        {
            quality = GetNumberOfWrongClassyfy(ruleSet);
        }

        private void Train()
        {
            GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm(dataSet.trainingSet, dataSet.headerRow, dataSet.decisionHeader);
            ruleSet = greedyAlgorithm.ruleSet;
        }

        private int GetNumberOfWrongClassyfy(RuleSet ruleSet)
        {
            int numberOfWrong = 0;
            foreach (var example in dataSet.testSet)
            {
                if (example[dataSet.decisionHeader] != ruleSet.GetListClassyfy(example))
                {
                    numberOfWrong++;
                }
            }
            return numberOfWrong;
        }
    }
}
  