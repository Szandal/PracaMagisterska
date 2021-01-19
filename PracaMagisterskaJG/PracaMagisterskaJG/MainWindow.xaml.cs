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
                LoadCsv(filePath);
                List<int> seeds = GetSeeds();
                for(int i = 0; i<30; i++)
                {
                    dataSetsList.Add(new DataSet(csvReader, seeds[i]));
                }
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
    }
}
