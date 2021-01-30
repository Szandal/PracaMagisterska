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

        private CsvReader csvReader;
        private List<DataSet> dataSetsList; 
        public MainWindow()
        {
            InitializeComponent();
        }
        string filePath = string.Empty;
        private void menuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "pliki CSV (*.csv) |*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                dataSetsList = new List<DataSet>();
                filePath = openFileDialog.FileName;
                tbFileName.Text = filePath;
                List<int> seeds = GetSeeds();
                for (int i = 0; i<30; i++)
                {
                    LoadCsv(filePath);
                    csvReader.Read();
                    csvReader.ReadHeader();
                    dataSetsList.Add(new DataSet(csvReader, seeds[i]));
                    csvReader.Context.CurrentIndex = 0;
                }
            }
        }

        private List<TrainValidateTest> UseTrainValidateTest()
        {
            List<TrainValidateTest> TVTList = new List<TrainValidateTest>();
            try
            {
                foreach (DataSet dataSet in dataSetsList)
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
            List<TrainValidateTest> TVTList = UseTrainValidateTest();
            TrainValidateTest bestTVT = GetBestQualityTVT(TVTList);
            string rulesToPrint = bestTVT.validateRuleSet.PrintRules();
            tbTest.Text = rulesToPrint;
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
    }
}
