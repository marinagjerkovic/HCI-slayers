using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PrviProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            currency_datagrid.ItemsSource = Currency.generateData();
            crypto_datagrid.ItemsSource = Currency.generateData();
            stocks_datagrid.ItemsSource = Currency.generateData();


        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            //LoadJSON client = new LoadJSON();
            //client.endPoint = JSONlink.Text;

            //string response = string.Empty;
            //response = client.makeRequest();

            //JSONoutput.Text = response;

        }
    }
}
