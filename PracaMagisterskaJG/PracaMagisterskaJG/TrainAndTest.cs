using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class TrainAndTest : TrainMethod
    {
        public DataSetTT dataSet;
        public TrainAndTest(DataSetTT dataSet)
        {
            this.dataSet = dataSet;
            Train();
            Test();
        }
        override
        protected void Test()
        {
            quality = Math.Round(ruleSet.GetNumberOfWrongClassyfy(dataSet.testSet) / (double)dataSet.testSet.Count,3);
        }
        override
        protected void Train()
        {
            GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm(dataSet.trainingSet, dataSet.headerRow, dataSet.decisionHeader);
            ruleSet = greedyAlgorithm.ruleSet;
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
                    setToExport = dataSet.testSet;
                    break;
                default:
                    return;
            }
            ExportCSV.SaveSetToFile(setToExport, dataSet.headerRow);
        }

    }
}
  