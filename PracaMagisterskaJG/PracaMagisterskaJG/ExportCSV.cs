using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaMagisterskaJG
{
    static class ExportCSV
    {
        public static void SaveSetToFile(List<Dictionary<string, string>> setToExport, string[] headerRow)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.Filter = "CSV file|*.csv";
            sFD.Title = "Zapisz do pliku zbiór danych";
            sFD.ShowDialog();
            if (sFD.FileName != "")
            {
                string header = "";
                foreach (string head in headerRow)
                {
                    header += head + ",";
                }
                header = header.TrimEnd(new Char[] { ',' });
                using (StreamWriter outputFile = new StreamWriter(sFD.FileName))
                {
                    outputFile.WriteLine(header);
                    foreach (var record in setToExport)
                    {
                        outputFile.WriteLine(DictionaryToCSVLine(record, headerRow));
                    }
                }

            }

        }
        private static string DictionaryToCSVLine(Dictionary<string, string> record, string[] headerRow)
        {
            string result = "";
            foreach (string head in headerRow)
            {
                result += record[head] + ",";
            }
            result = result.TrimEnd(new Char[] { ',' });
            return result;
        }

        internal static void ExportRuleSet(RuleSet ruleSet)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.Filter = "CSV file|*.csv|Text file|*.txt";
            sFD.Title = "Zapisz do pliku zbiór reguł";
            sFD.ShowDialog();
            if (sFD.FileName != "")
            {
                using (StreamWriter outputFile = new StreamWriter(sFD.FileName))
                {
                    foreach (var rule in ruleSet.GetRuleSet())
                    {
                        outputFile.WriteLine(rule.PrintRuleToCSV());
                    }
                }

            }
        }

    }
}
