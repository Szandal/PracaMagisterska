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
using System.Windows.Shapes;

namespace PracaMagisterskaJG
{
    /// <summary>
    /// Logika interakcji dla klasy NumberOfSetsDialog.xaml
    /// </summary>
    public partial class NumberOfSetsDialog : Window
    {
        public NumberOfSetsDialog()
        {
            InitializeComponent();
        }
        public int Answer
        {
            get { return int.Parse(tbNumberOfSets.Text); }
        }
        private void tbNumberOfSets_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            if (int.TryParse(tbNumberOfSets.Text, out temp))
            {
                tbNumberOfSets.Background = Brushes.White;
            }
            else
            {
                tbNumberOfSets.Background = Brushes.Red;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (tbNumberOfSets.Background == Brushes.Red)
            {
                MessageBox.Show("Nieprawidłowa wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (tbNumberOfSets.Text == "")
                {
                    MessageBox.Show("Nieprawidłowa wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    this.DialogResult = true;
                }
            }
        }
    }
}
