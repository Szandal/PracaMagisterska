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
    /// Logika interakcji dla klasy PercentDialog.xaml
    /// </summary>
    public partial class PercentDialog : Window
    {
        public double Answer
        {
            get { return double.Parse(tbPercent.Text); }
        }
        public PercentDialog()
        {
            InitializeComponent();
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (tbPercent.Background == Brushes.Red)
            {
                MessageBox.Show("Nieprawidłowa wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(tbPercent.Text == "")
                {
                    MessageBox.Show("Nieprawidłowa wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    this.DialogResult = true;
                }
            }
        }

        private void tbPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (double.TryParse(tbPercent.Text, out temp))
            {
                if (temp < 0 || temp > 100)
                {
                    tbPercent.Background = Brushes.Red;
                }
                else
                {
                    tbPercent.Background = Brushes.White;
                }
            }
            else
            {
                tbPercent.Background = Brushes.Red;
            }
        }
    }
}
