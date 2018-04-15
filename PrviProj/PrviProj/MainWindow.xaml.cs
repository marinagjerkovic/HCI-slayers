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
using Newtonsoft.Json;
using System.Windows.Forms.Integration;
using LiveCharts;
using LiveCharts.Wpf;

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

        public ObservableCollection<ShowingCurrencyClass> shownCurrenciesList { get; set; }
        

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

            //ucitajIspisiJSON();

            value = new Dictionary<int, double>();
            for (int i = 0; i < 10; i++)
                value.Add(i, 10 * i);

            shownCurrenciesList = new ObservableCollection<ShowingCurrencyClass>();
            //ucitajIspisiJSON();
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
                    //ovde ubacujem da se trazi vrednost u odnosu na referentnu valutu, nek je defaultna USD
                    //i da se updatuje na svakih 10 sec

                    c.Client = new LoadJSON();
                        
                    c.startTiming(10);
                    chosenDigitalList.Insert(0, c);
                    break;   
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
                    c.Client = new LoadJSON();
                    c.startTiming(10);

                    chosenPhysicalList.Insert(0, c);
                    break;
                }
            }
        }

        void ShowHideDetails(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    //vis.
                    row.DetailsVisibility =
                    row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
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
                    //za deonice ne moze da se dobije exchange rate sa alpha vantage-a
                    chosenStockList.Insert(0, c);
                    break;
                }
            }
        }

        //FUNKCIJE KOJE SE POZIVAJU SA REMOVE BUTTONA
        private void removeDigitalButton(Object sender, RoutedEventArgs e)
        {
            chosenDigitalList.Remove((CurrencyClass)dataGridDigital.SelectedItem);
            

        }

        private void removePhysicalButton(Object sender, RoutedEventArgs e)
        {
            chosenPhysicalList.Remove((CurrencyClass)dataGridDigital.SelectedItem);

        }

        private void removeStockButton(Object sender, RoutedEventArgs e)
        {
            chosenStockList.Remove((CurrencyClass)dataGridDigital.SelectedItem);

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



        private void dataGridDigital_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;

                        string symbol = ((CurrencyClass)dgr.Item).Symbol;

                        //ovde ce se prosledjivati interval u zavisnosti od toga koji je korisnik izabrao
                        loadJSON(CurrencyIntervalType.DIGITAL_CURRENCY_MONTHLY, symbol);
                    }
                }
            }
        }

        private void dataGridPhysicalStock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;

                        string symbol = ((CurrencyClass)dgr.Item).Symbol;

                        //ovde ce se prosledjivati interval u zavisnosti od toga koji je korisnik izabrao
                        loadJSON(CurrencyIntervalType.TIME_SERIES_MONTHLY, symbol);
                    }
                }
            }
        }

        

        //npr DIGITAL_CURRENCY_MONTHLY, BTC -> za mesecni bitcoin 
        private void loadJSON(CurrencyIntervalType interval, string symbol)
        {
            LoadJSON client = new LoadJSON();
            string link = string.Empty;
            string keyForTimeseries = string.Empty;

            switch (interval)
            {
                case CurrencyIntervalType.DIGITAL_CURRENCY_INTRADAY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_INTRADAY&symbol="+symbol+ "&market=CNY&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Intraday)";
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_DAILY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_DAILY&symbol="+symbol+"&market=CNY&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Daily)";
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_WEEKLY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_WEEKLY&symbol="+symbol+"&market=CNY&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Weekly)";
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_MONTHLY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_MONTHLY&symbol="+symbol+"&market=CNY&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Monthly)";
                    break;
                case CurrencyIntervalType.TIME_SERIES_INTRADAY:
                    link = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + symbol + "&interval=15min&outputsize=full&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (15min)";
                    break;
                case CurrencyIntervalType.TIME_SERIES_DAILY:
                    link = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + symbol + "&outputsize=full&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Daily)";
                    break;
                case CurrencyIntervalType.TIME_SERIES_WEEKLY:
                    link = "https://www.alphavantage.co/query?function=TIME_SERIES_WEEKLY&symbol="+symbol+ "&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Weekly Time Series";
                    break;              
                default:
                    link = "https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol="+symbol+ "&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Monthly Time Series";
                    break;
            }

            client.endPoint = link;

            string response = string.Empty;
            response = client.makeRequest();
            var jObj = JsonConvert.DeserializeObject<dynamic>(response);
            ShowingCurrencyClass showingCurrency = new ShowingCurrencyClass();

            showingCurrency.Metadata = jObj["Meta Data"].ToObject<Dictionary<string, string>>();
            showingCurrency.Type = interval;

            //u zavisnosti od intervala se prosledjuje kljuc za timeseries
            showingCurrency.Timeseries = jObj[keyForTimeseries].ToObject<Dictionary<string, Dictionary<string, string>>>();

            shownCurrenciesList.Add(showingCurrency);



            TabItem tabItem = new TabItem();
                
            tabItem.Header = symbol;
            CartesianChart cart = new CartesianChart();

            ChartValues<double> vals = new ChartValues<double>();
            List<string> labels = new List<string>();

            foreach (string key in showingCurrency.Timeseries[showingCurrency.Timeseries.Keys.ElementAt(0)].Keys)
            {
                Double a = Double.Parse(showingCurrency.Timeseries[showingCurrency.Timeseries.Keys.ElementAt(0)][key]);
                vals.Add(a);
                //ne znam sta sa podacima, ovo je samo probno
            }

            foreach (string key in showingCurrency.Timeseries.Keys)
            {
                labels.Add(key);
            }

            LiveCharts.SeriesCollection sers = new LiveCharts.SeriesCollection
            {
            new LineSeries
            {
                Title = "Series 1",
                Values = vals
            }
        };

            //cart.LegendLocation = LiveCharts.LegendLocation.Right;
            cart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = labels,
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 1,
                    IsEnabled = false
                }
            });

            cart.Series = sers;
            cart.ScrollMode = LiveCharts.ScrollMode.XY;

            tabItem.Content = cart;
            //ScrollViewer skrol = new ScrollViewer();
            //skrol.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            //skrol.Content = cart;
            tabItem.Content = cart;
            tabControl.Items.Add(tabItem);
            tabControl.SelectedItem = tabItem;
        
           

        }


        


    }
}
