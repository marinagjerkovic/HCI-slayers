using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace PrviProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        Dictionary<int, double> value;

        private CSVParser parser = new CSVParser();
        public ObservableCollection<CurrencyClass> physicalCurrenciesList { get; set; }
        public ObservableCollection<CurrencyClass> digitalCurrenciesList { get; set; }
        public ObservableCollection<CurrencyClass> stocksList { get; set; }

        public ObservableCollection<CurrencyClass> chosenDigitalList { get; set; }
        public ObservableCollection<CurrencyClass> chosenPhysicalList { get; set; }
        public ObservableCollection<CurrencyClass> chosenStockList { get; set; }


        public MainWindow()
        {
            //var chart = new LineChart();
            InitializeComponent();
            this.DataContext = this;

            digitalCurrenciesList = parser.parse("digital_currencies.txt");
            physicalCurrenciesList = parser.parse("physical_currencies.txt");
            stocksList = parser.parse("stocks.txt");

            chosenDigitalList = new ObservableCollection<CurrencyClass>();
            chosenPhysicalList = new ObservableCollection<CurrencyClass>();
            chosenStockList = new ObservableCollection<CurrencyClass>();

            ucitajIspisiJSON();

            value = new Dictionary<int, double>();
            for (int i = 0; i < 10; i++)
                value.Add(i, 10 * i);

            Chart chart = this.FindName("MyWinformChart") as Chart;
            chart.DataSource = value;
            chart.Series["series"].XValueMember = "Key";
            chart.Series["series"].YValueMembers = "Value";

            Chart chart2 = this.FindName("MyWinformChart2") as Chart;
            chart2.DataSource = value;
            chart2.Series["series"].XValueMember = "Key";
            chart2.Series["series"].YValueMembers = "Value";
        }

        //za prikaz u comboboxu
        private List<string> data2ListStrings(ObservableCollection<CurrencyClass> collection)
        {
            List<string> list = new List<string>();
            foreach (var x in collection)
            {
                list.Add(String.Format("{0} ({1})", x.Name, x.Symbol));
            }

            return list;
        }


        private void ComboBox_Loaded_Digital(object sender, RoutedEventArgs e)
        {


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data2ListStrings(digitalCurrenciesList);

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }


        private void ComboBox_Loaded_Physical(object sender, RoutedEventArgs e)
        {


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data2ListStrings(physicalCurrenciesList);

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }

        private void ComboBox_Loaded_Stocks(object sender, RoutedEventArgs e)
        {


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data2ListStrings(stocksList);

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }


        private void AddCurrencyDigital(object sender, RoutedEventArgs e)
        {

            string name = ((sender as CheckBox).Content as string);
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in digitalCurrenciesList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    chosenDigitalList.Insert(0,c);
                }
            }
        }

        private void AddCurrencyPhysical(object sender, RoutedEventArgs e)
        {

            string name = ((sender as CheckBox).Content as string);
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in physicalCurrenciesList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    chosenPhysicalList.Insert(0, c);
                }
            }
        }


        private void AddStock(object sender, RoutedEventArgs e)
        {

            string name = ((sender as CheckBox).Content as string);
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in stocksList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    chosenStockList.Insert(0, c);
                }
            }
        }


        private void RemoveCurrencyDigital(object sender, RoutedEventArgs e)
        {

            string name = ((sender as CheckBox).Content as string);
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in chosenDigitalList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    chosenDigitalList.Remove(c);
                    break;
                }
            }
        }

        private void RemoveCurrencyPhysical(object sender, RoutedEventArgs e)
        {

            string name = ((sender as CheckBox).Content as string);
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in chosenPhysicalList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    chosenPhysicalList.Remove(c);
                    break;
                }
            }
        }

        private void RemoveStock(object sender, RoutedEventArgs e)
        {

            string name = ((sender as CheckBox).Content as string);
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in chosenStockList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    chosenStockList.Remove(c);
                    break;
                }
            }
        }


        //ovo treba da se izvrsi kad se klikne na neku valutu
        private void load_Click(object sender, RoutedEventArgs e)
        {
            LoadJSON client = new LoadJSON();
            //client.endPoint = JSONlink.Text;

            string response = string.Empty;
            response = client.makeRequest();

            //JSONoutput.Text = response;

        }


        //proba
        private void ucitajIspisiJSON()
        {
            LoadJSON client = new LoadJSON();
            client.endPoint = "https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol=MSFT&apikey=demo";

            string response = string.Empty;
            response = client.makeRequest();
            textBox.Text = response;
            //JSONoutput.Text = response;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
