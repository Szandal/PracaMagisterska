 using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class DataSetTVT: DataSet
    {
        public List<Dictionary<string, string>> trainingSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> validationSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> testSet = new List<Dictionary<string, string>>(); 

        public DataSetTVT(CsvReader csvReader, int seed)
        {
            ReadEntire(csvReader);
            Random random = new Random(seed);
            SplitSet(random);

        }

        private void SplitSet(Random random)
        {
            foreach(var record in entireSet)
            {
                double rand = random.NextDouble();
                if (rand < 0.3)
                {
                    trainingSet.Add(record);
                }else if(rand < 0.5)
                {
                    validationSet.Add(record);
                }
                else
                {
                    testSet.Add(record);
                }
            }
            if(GreedyAlgorithm.getUncertaintyOfSubset(trainingSet,decisionHeader)==0 || GreedyAlgorithm.getUncertaintyOfSubset(validationSet, decisionHeader) == 0 || GreedyAlgorithm.getUncertaintyOfSubset(testSet, decisionHeader) == 0)
            {
                trainingSet = new List<Dictionary<string, string>>();
                validationSet = new List<Dictionary<string, string>>();
                testSet = new List<Dictionary<string, string>>();
                SplitSet(random);  
            }
        }
    }
}
