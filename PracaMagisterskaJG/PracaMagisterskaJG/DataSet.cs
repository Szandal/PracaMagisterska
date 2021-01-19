using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class DataSet
    {
        public List<Dictionary<string, string>> entireSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> trainingSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> validationSet = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> testSet = new List<Dictionary<string, string>>();
        public string[] headerRow;
        public string decisionHeader; 

        public DataSet(CsvReader csvReader, int seed)
        {
            ReadEntire(csvReader);
            SplitSet(seed);

        }

        private void ReadEntire(CsvReader csvReader)
        {
            string value;
            csvReader.Read();
            csvReader.ReadHeader();
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

        private void SplitSet(int seed)
        {
            Random random = new Random(seed);
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
        }
    }
}
