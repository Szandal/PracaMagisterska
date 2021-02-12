using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class DataSetTT
    {
        public List<Dictionary<string, string>> entireSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> trainingSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> testSet = new List<Dictionary<string, string>>();
        public string[] headerRow;
        public string decisionHeader;

        public DataSetTT(CsvReader csvReader,double percent, int seed)
        {
            ReadEntire(csvReader);
            Random random = new Random(seed);
            SplitSet(random, percent);

        }

        private void ReadEntire(CsvReader csvReader)
        {
            string value;
            //csvReader.Read();
            //csvReader.ReadHeader();
            headerRow = csvReader.Context.HeaderRecord;
            decisionHeader = headerRow[headerRow.Length - 1];
            while (csvReader.Read())
            {
                Dictionary<string, string> record = new Dictionary<string, string>();
                for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
                {
                    record.Add(headerRow[i], value);
                }
                entireSet.Add(record);
            }
        }

        private void SplitSet(Random random, double percent)
        {
            double threshold = percent / 100;
            foreach (var record in entireSet)
            {
                double rand = random.NextDouble();
                if (rand < threshold)
                {
                    trainingSet.Add(record);
                }
                else
                {
                    testSet.Add(record);
                }
            }
            if (GreedyAlgorithm.getUncertaintyOfSubset(trainingSet, decisionHeader) == 0 || GreedyAlgorithm.getUncertaintyOfSubset(testSet, decisionHeader) == 0)
            {
                trainingSet = new List<Dictionary<string, string>>();
                testSet = new List<Dictionary<string, string>>();
                SplitSet(random, percent);
            }
        }
    }

}
