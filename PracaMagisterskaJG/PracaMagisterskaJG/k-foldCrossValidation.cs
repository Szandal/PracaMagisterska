using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class k_foldCrossValidation : ValidationMetod
    {
        public DataSetCross dataSet;
        public List<double> qualityList;
        private List<RuleSet> ruleSetList;
        
        public double bestQuality;
        public int bestIndex;
        public k_foldCrossValidation(DataSetCross dataSet)
        {
            ruleSetList = new List<RuleSet>();
            qualityList = new List<double>();
            this.dataSet = dataSet;
            //Parallel.For(0, dataSet.numberOfSets, i =>
            //{
            //    List<Dictionary<string, string>> trainingSet = dataSet.GetTrainingSet(i);
            //    GreedyAlgorithm greedy = new GreedyAlgorithm(trainingSet, dataSet.headerRow, dataSet.decisionHeader);
            //    RuleSet temp = greedy.ruleSet;
            //    ruleSetList.Add(temp);
            //    qualityList.Add(GetQualityOfRuleSet(i, temp));
            //}
            //);
            for(int i = 0; i< dataSet.numberOfSets; i++)
            {
                List<Dictionary<string, string>> trainingSet = dataSet.GetTrainingSet(i);
                GreedyAlgorithm greedy = new GreedyAlgorithm(trainingSet, dataSet.headerRow, dataSet.decisionHeader);
                RuleSet temp = greedy.ruleSet;
                ruleSetList.Add(temp);
                qualityList.Add(GetQualityOfRuleSet(i, temp));
            }
            SetAVGQuality();
            SetBestQuality();

        }

        private void SetBestQuality()
        {
            int temp = 0;
            for(int q = 0; q<qualityList.Count; q++)
            {
                if (qualityList[q] < temp)
                    temp = q;
            }
            bestQuality = qualityList[temp];
            bestIndex = temp;
            ruleSet = ruleSetList[bestIndex];
        }

        private void SetAVGQuality()
        {
            int sum = 0;
            foreach(int q in qualityList)
            {
                sum += q;
            }
            quality = (double)sum / (double)dataSet.numberOfSets;
        }

        private double GetQualityOfRuleSet(int i, RuleSet rules)
        {
            int numberOfWrong = 0;
            foreach (var example in dataSet.ListOfSets[i])
            {
                if (example[dataSet.decisionHeader] != rules.GetListClassyfy(example))
                {
                    numberOfWrong++;
                }
            }
            return Math.Round( (double)numberOfWrong / (double)dataSet.ListOfSets[i].Count, 3);
        }

        internal void ExportSet(int set)
        {
            List<Dictionary<string, string>> setToExport;
            switch (set)
            {
                case 0:
                    setToExport = dataSet.GetTrainingSet(bestIndex);
                    break;
                case 1:
                    setToExport = dataSet.ListOfSets[bestIndex];
                    break;
                default:
                    return;
            }
            ExportCSV.SaveSetToFile(setToExport, dataSet.headerRow);
        }

        internal void ExportRules()
        {
            ExportCSV.ExportRuleSet(ruleSet);
        }
    }
}
