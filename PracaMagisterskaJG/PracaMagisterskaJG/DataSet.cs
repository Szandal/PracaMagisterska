using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    abstract class DataSet
    {
        public List<Dictionary<string, string>> entireSet = new List<Dictionary<string, string>>();
        public string[] headerRow;
        public string decisionHeader;


        protected void ReadEntire(CsvReader csvReader)
        {
            string value;
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

    }
}
