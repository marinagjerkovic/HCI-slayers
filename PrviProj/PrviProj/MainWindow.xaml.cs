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
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PrviProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CSVParser parser = new CSVParser();
        public ObservableCollection<CurrencyClass> physicalCurrenciesList { get; set; }
        public ObservableCollection<CurrencyClass> digitalCurrenciesList { get; set; }
        public ObservableCollection<CurrencyClass> stocksList { get; set; }

        public ObservableCollection<CurrencyClass> chosenDigitalList { get; set; }
        public ObservableCollection<CurrencyClass> chosenPhysicalList { get; set; }
        public ObservableCollection<CurrencyClass> chosenStockList { get; set; }


        public MainWindow()
        {
            
            InitializeComponent();
            this.DataContext = this;

            digitalCurrenciesList = parser.parse("digital_currencies.txt");
            physicalCurrenciesList = parser.parse("physical_currencies.txt");
            stocksList = parser.parse("stocks.txt");

            chosenDigitalList = new ObservableCollection<CurrencyClass>();
            chosenPhysicalList = new ObservableCollection<CurrencyClass>();
            chosenStockList = new ObservableCollection<CurrencyClass>();
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
                    chosenDigitalList.Add(c);
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
                    chosenPhysicalList.Add(c);
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
                    chosenStockList.Add(c);
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
    }
}
