using CsvHelper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    class DataSetCross : DataSet
    {
        public List<List<Dictionary<string, string>>> ListOfSets;
        public int numberOfSets;

        public DataSetCross(CsvReader csvReader, int numberOfSets, int seed)
        {
            this.numberOfSets = numberOfSets;
            ReadEntire(csvReader);
            Random random = new Random(seed);
            SplitSet(random, numberOfSets);
        }

        private void SplitSet(Random random, int numberOfSets)
        {
            CreateLists(numberOfSets);
            List<Dictionary<string, string>> Set = CopySet(entireSet);
            int i = 0;
            do
            {
                int ran = random.Next(Set.Count);
                ListOfSets[i].Add(Set[ran]);
                Set.RemoveAt(ran);
                i = (i + 1) % ListOfSets.Count;
            } while (Set.Count > 0);

            //if (numberOfSets == entireSet.Count) //leave-one-out
            //{
            //    for(int i=0; i < numberOfSets; i++)
            //    {
            //        ListOfSets[i].Add(entireSet[i]);
            //    }
            //}
            //else
            //{
            //    foreach (var record in entireSet)
            //    {
            //        int setNumber = random.Next(numberOfSets);
            //        ListOfSets[setNumber].Add(record);
            //    }
            //    ValidateSplitedSets(random, numberOfSets);
            //}

        }

        private List<Dictionary<string, string>> CopySet(List<Dictionary<string, string>> entireSet)
        {
            List<Dictionary<string, string>> copy = new List<Dictionary<string, string>>();
            foreach (Dictionary<string, string> line in entireSet)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();
                foreach(string key in line.Keys)
                {
                    temp.Add(key, line[key]);
                }
                copy.Add(temp);
            }
            return copy;
        }



        private void CreateLists(int numberOfSets)
        {
            ListOfSets = new List<List<Dictionary<string, string>>>();
            for (int i = 0; i < numberOfSets; i++)
            {
                ListOfSets.Add(new List<Dictionary<string, string>>());
            }
        }

        public List<Dictionary<string, string>> GetTrainingSet(int i)
        {
            List<Dictionary<string, string>> trainingSet = new List<Dictionary<string, string>>();

            //Parallel.For(0, ListOfSets.Count-1, j =>
            for(int j=0; j<ListOfSets.Count; j++)
            {
                if (j != i)
                {
                    foreach (var record in ListOfSets[j])
                        trainingSet.Add(record);
                }
            }
            /*)*/;
            return trainingSet;
        }
    }
}