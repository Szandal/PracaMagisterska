using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace PracaMagisterskaJG
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum operation
        {
            none,
            TrainAndTest,
            TrainValidateTest,
            CrossValidate
        };

        private operation lastOperation;
        private CsvReader csvReader;
        private List<DataSetTVT> TVTDataSetsList;
        private List<DataSetTT> TTDataSetsList;
        private List<DataSetCross> CrossDataSetsList;
        private TrainAndTest bestTT;
        private TrainValidateTest bestTVT;
        private k_foldCrossValidation bestCross;
        public MainWindow()
        {
            InitializeComponent();
            lastOperation = operation.none;
        }
        string filePath = string.Empty;
        private void menuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            TVTDataSetsList = null;
            TTDataSetsList = null;
            CrossDataSetsList = null;
            tbTest.Text = "";
            tbSizeOfSet.Text = "";
            tbSizeOfBestValidateSet.Text = "";
            tbSizeOfBestTreningSet.Text = "";
            tbSizeOfBestTestSet.Text = "";
            tbSizeOfBestruleSet.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "pliki CSV (*.csv) |*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                
                filePath = openFileDialog.FileName;
                tbFileName.Text = filePath;
            }
        }
        private List<TrainValidateTest> UseTrainValidateTest()
        {
            LoadTVTDataSetList();
            List<TrainValidateTest> TVTList = new List<TrainValidateTest>();
            try
            {
                foreach (DataSetTVT dataSet in TVTDataSetsList)
                {
                    TVTList.Add(new TrainValidateTest(dataSet));
                }
            }
            catch
            {
                FileNotFound();
            }
            return TVTList;
        }
        private void LoadTVTDataSetList()
        {
            List<int> seeds = GetSeeds();
            for (int i = 0; i < 30; i++)
            {
                LoadCsv(filePath);
                csvReader.Read();
                csvReader.ReadHeader();
                TVTDataSetsList.Add(new DataSetTVT(csvReader, seeds[i]));
                csvReader.Context.CurrentIndex = 0;
            }
        }
        private void LoadTTDataSetList(double percent)
        {
            List<int> seeds = GetSeeds();
            for (int i = 0; i < 30; i++)
            {
                LoadCsv(filePath);
                csvReader.Read();
                csvReader.ReadHeader();
                TTDataSetsList.Add(new DataSetTT(csvReader,percent,seeds[i]));
                csvReader.Context.CurrentIndex = 0;
            }
        }
        private List<int> GetSeeds()
        {
            List<int> seedsList = new List<int>();
            try
            {
                string[] lines = File.ReadAllLines("seeds.txt");
                foreach(string line in lines)
                {
                    seedsList.Add(int.Parse(line));
                }
            }
            catch
            {
                MessageBox.Show("Błąd pliku seeds.txt");
            }
            return seedsList;
        }
        private void LoadCsv(string filePath)
        {
            var streamReader = File.OpenText(filePath);
            var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
            csvReader.Configuration.Delimiter = ",";
            csvReader.Configuration.HasHeaderRecord = true;
            this.csvReader = csvReader;
        }
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void menuItemTVT_Click(object sender, RoutedEventArgs e)
        {
            lastOperation = operation.TrainValidateTest;
            TVTDataSetsList = new List<DataSetTVT>();
            List<TrainValidateTest> TVTList = UseTrainValidateTest();
            bestTVT = GetBestQualityTVT(TVTList);
            string messageToPrint = PrintQualityInfo(TVTList, bestTVT);
            messageToPrint += bestTVT.ruleSet.PrintRules();
            tbTest.Text = messageToPrint;
            tbSizeOfSet.Text = "Wielkość zbioru uczącego: " + bestTVT.dataSet.entireSet.Count();
            tbSizeOfBestTreningSet.Text = "Wielkość najlepszego zbioru treningowego: " + bestTVT.dataSet.trainingSet.Count();
            tbSizeOfBestValidateSet.Text = "Wielkość najlepszego zbioru validującego: " + bestTVT.dataSet.validationSet.Count();
            tbSizeOfBestTestSet.Text = "Wielkość najlepszego zbioru testowego: " + bestTVT.dataSet.testSet.Count();
            tbSizeOfBestruleSet.Text = "Ilość reguł: " + bestTVT.ruleSet.GetRuleSet().Count();
        }
        private string PrintQualityInfo(List<TrainValidateTest> TVTList, TrainValidateTest bestTVT)
        {
            string message = "Średnia jakość klasyfikatora wyniosła: " + GetAVGQuality(TVTList) + "\n";
            message += "Najlepsza jakość klasyfikatora wyniosła: " + bestTVT.quality.ToString() + "\n";
            List<double> qualities = new List<double>();
            foreach (var TVT in TVTList)
            {
                qualities.Add(TVT.quality);
            }
            message += "Odchylenie standardowe jakości klasyfikatora wyniosło: " + StandardDeviation(qualities) + "\n";
            return message;
        }
        private string GetAVGQuality(List<TrainValidateTest> TVTList)
        {
            double quality = 0; 
            foreach(var TVT in TVTList)
            {
                quality += TVT.quality;
            }
            return (Math.Round(quality / TVTList.Count(), 3)).ToString();
        }
        private TrainValidateTest GetBestQualityTVT(List<TrainValidateTest> TVTList)
        {
            int best = 0;
            for(int i =1; i<TVTList.Count; i++)
            {
                if(TVTList[best].quality > TVTList[i].quality)
                {
                    best = i;
                }
            }
            return TVTList[best];
        }
        private void FileNotFound()
        {
            MessageBox.Show("Nie załadowano pliku z danymi", "Error");
        }
        private void menuItemTT_Click(object sender, RoutedEventArgs e)
        {
            lastOperation = operation.TrainAndTest;
            double percent = 0;
            PercentDialog pd = new PercentDialog();
            if (pd.ShowDialog()== true)
            {
                percent = pd.Answer;
            }
            if(percent == 0)
            {
                return;
            }
            TTDataSetsList = new List<DataSetTT>();
            LoadTTDataSetList(percent);
            List<TrainAndTest> TTList = UseTrainAndTest();

            bestTT = GetBestQualityTT(TTList);
            string messageToPrint = PrintQualityInfo(TTList, bestTT);
            messageToPrint += bestTT.ruleSet.PrintRules();
            tbTest.Text = messageToPrint;
            tbSizeOfSet.Text = "Wielkość zbioru uczącego: " + bestTT.dataSet.entireSet.Count();
            tbSizeOfBestTreningSet.Text = "Wielkość najlepszego zbioru treningowego: " + bestTT.dataSet.trainingSet.Count();
            tbSizeOfBestValidateSet.Text = "";
            tbSizeOfBestTestSet.Text = "Wielkość najlepszego zbioru testowego: " + bestTT.dataSet.testSet.Count();
            tbSizeOfBestruleSet.Text = "Ilość reguł: " + bestTT.ruleSet.GetRuleSet().Count();
        }
        private string PrintQualityInfo(List<TrainAndTest> TTList, TrainAndTest bestTT)
        {
            string message = "Średnia jakość klasyfikatora wyniosła: " + GetAVGQuality(TTList) + "\n";
            message += "Najlepsza jakość klasyfikatora wyniosła: " + bestTT.quality.ToString() + "\n";
            List<double> qualities = new List<double>();
            foreach(var TT in TTList)
            {
                qualities.Add(TT.quality);
            }
            message += "Odchylenie standardowe jakości klasyfikatora wyniosło: " + StandardDeviation(qualities) + "\n";
            return message;
        }
        private string GetAVGQuality(List<TrainAndTest> TTList)
        {
            double quality = 0;
            foreach (var TT in TTList)
            {
                quality += TT.quality;
            }
            return (quality / TTList.Count()).ToString();
        }
        private TrainAndTest GetBestQualityTT(List<TrainAndTest> TTList)
        {
            int best = 0;
            for (int i = 1; i < TTList.Count; i++)
            {
                if (TTList[best].quality > TTList[i].quality)
                {
                    best = i;
                }
            }
            return TTList[best];
        }
        private List<TrainAndTest> UseTrainAndTest()
        {
            List<TrainAndTest> TTList = new List<TrainAndTest>();
            try
            {
                Parallel.ForEach(TTDataSetsList, dataSet =>
              {
                  TTList.Add(new TrainAndTest(dataSet));
              }
                );
            }
            catch
            {
                FileNotFound();
            }
            return TTList;
        }
        private void menuItemCross_Click(object sender, RoutedEventArgs e)
        {
            lastOperation = operation.CrossValidate;
            int numberOfSets = 0;
            NumberOfSetsDialog pd = new NumberOfSetsDialog();
            if (pd.ShowDialog() == true)
            {
                numberOfSets = pd.Answer;
            }
            if (numberOfSets == 0)
            {
                return;
            }
            CrossDataSetsList = new List<DataSetCross>();
            LoadCrossDataSetList(numberOfSets);
            List<k_foldCrossValidation> kList = UseKFoldCrossValidation();

            bestCross = GetBestQualityCross(kList);
            string messageToPrint = PrintQualityInfo(kList, bestCross);
            messageToPrint += bestCross.ruleSet.PrintRules();
            tbTest.Text = messageToPrint;
            tbSizeOfSet.Text = "Wielkość zbioru uczącego: " + bestCross.dataSet.entireSet.Count();
            tbSizeOfBestTreningSet.Text = "";
            tbSizeOfBestValidateSet.Text = "";
            tbSizeOfBestTestSet.Text = "";
            tbSizeOfBestruleSet.Text = "Ilość reguł: " + bestCross.ruleSet.GetRuleSet().Count();
        }
        private string PrintQualityInfo(List<k_foldCrossValidation> kList, k_foldCrossValidation bestCross)
        {
            string message = "Średnia jakość klasyfikatora wyniosła: " + GetAVGQuality(kList) + "\n";
            message += "Najlepsza jakość klasyfikatora wyniosła: " + bestCross.quality.ToString() + "\n";
            List<double> qualities = new List<double>();
            foreach (var k in kList)
            {
                qualities.Add(k.quality);
            }
            message += "Odchylenie standardowe jakości klasyfikatora wyniosło: " + StandardDeviation(qualities) + "\n";
            return message;
        }
        private string GetAVGQuality(List<k_foldCrossValidation> kList)
        {
            double quality = 0;
            foreach (var k in kList)
            {
                quality += k.quality;
            }
            return (Math.Round(quality / kList.Count(), 3)).ToString();
        }
        private k_foldCrossValidation GetBestQualityCross(List<k_foldCrossValidation> kList)
        {
            int best = 0;
            for (int i = 1; i < kList.Count; i++)
            {
                if (kList[best].quality > kList[i].quality)
                {
                    best = i;
                }
            }
            return kList[best];
        }
        private void LoadCrossDataSetList(int numberOfSets)
        {
            List<int> seeds = GetSeeds();
            for (int i = 0; i < 30; i++)
            {
                LoadCsv(filePath);
                csvReader.Read();
                csvReader.ReadHeader();
                CrossDataSetsList.Add(new DataSetCross(csvReader, numberOfSets, seeds[i]));
                csvReader.Context.CurrentIndex = 0;
            }
        }
        private List<k_foldCrossValidation> UseKFoldCrossValidation()
        {
            List<k_foldCrossValidation> crossList = new List<k_foldCrossValidation>();
            //crossList.Add(new k_foldCrossValidation(CrossDataSetsList[0]));
            try
            {
                // Parallel.ForEach(CrossDataSetsList, dataSet =>
                //{
                //    crossList.Add(new k_foldCrossValidation(dataSet));
                //}
                // );
                foreach (var dataSet in CrossDataSetsList)
                {
                    crossList.Add(new k_foldCrossValidation(dataSet));
                }
            }
            catch
            {
                FileNotFound();
            }
            return crossList;
        }
        private void exportTrain_Click(object sender, RoutedEventArgs e)
        {
            switch (lastOperation)
            {
                case operation.none:
                    MessageBox.Show("Żadna operacja klasyfikacji nie została jeszcze wykonana", "Brak danych do eksportu", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case operation.TrainAndTest:
                    bestTT.ExportSet(0);
                    break;
                case operation.TrainValidateTest:
                    bestTVT.ExportSet(0);
                    break;
                case operation.CrossValidate:
                    bestCross.ExportSet(0);
                    break;
            }
        }

        private void exportValidate_Click(object sender, RoutedEventArgs e)
        {
            if(lastOperation == operation.TrainValidateTest)
            {
                bestTVT.ExportSet(1);
            }
            else
            {
                MessageBox.Show("W wybranej ostanio metodzie nie występuje zbiór walidacyjny", "Brak zbioru do eksportu", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void exportTest_Click(object sender, RoutedEventArgs e)
        {
            switch (lastOperation)
            {
                case operation.none:
                    MessageBox.Show("Żadna operacja klasyfikacji nie została jeszcze wykonana", "Brak danych do eksportu", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case operation.TrainAndTest:
                    bestTT.ExportSet(1);
                    break;
                case operation.TrainValidateTest:
                    bestTVT.ExportSet(2);
                    break;
                case operation.CrossValidate:
                    bestCross.ExportSet(1);
                    break;
            }
        }

        private void exportRule_Click(object sender, RoutedEventArgs e)
        {
            switch (lastOperation)
            {
                case operation.none:
                    MessageBox.Show("Żadna operacja klasyfikacji nie została jeszcze wykonana", "Brak danych do eksportu", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case operation.TrainAndTest:
                    bestTT.ExportRules();
                    break;
                case operation.TrainValidateTest:
                    bestTVT.ExportRules();
                    break;
                case operation.CrossValidate:
                    bestCross.ExportRules();
                    break;
            }
        }



        private double StandardDeviation(List<double> qualities)
        {
            double AVG = qualities.Average();
            List<double> upper = new List<double>();
            foreach(var q in qualities)
            {
                double temp = q - AVG;
                upper.Add(Math.Pow(temp,2));
            }
            double sum = qualities.Sum();
            double SDx2 = sum / (qualities.Count - 1);
            double SD = Math.Sqrt(SDx2);
            return Math.Round(SD, 3);
        }

    }
}
