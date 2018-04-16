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
using System.IO;

namespace PrviProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    
        //Dictionary<int, double> value;

        private CSVParser parser = new CSVParser();
        public ObservableCollection<CurrencyClass> physicalCurrenciesList { get; set; }
        public ObservableCollection<CurrencyClass> digitalCurrenciesList { get; set; }
        public ObservableCollection<CurrencyClass> stocksList { get; set; }

        public ObservableCollection<CurrencyClass> chosenDigitalList { get; set; }
        public ObservableCollection<CurrencyClass> chosenPhysicalList { get; set; }
        public ObservableCollection<CurrencyClass> chosenStockList { get; set; }

        public ObservableCollection<ShowingCurrencyClass> shownCurrenciesList { get; set; }

        public ObservableCollection<CurrencyClass> referentCurrenciesList { get; set; }
        public static CurrencyClass referentCurrency { get; set; }

        public static double updateInterval { get; set; }
        //historyIterval ne mora da se cuva, jer ce svaki objekat za sebe pamtiti kog je tipa
        public List<string> historyIntervalsList = new List<string> { "intraday", "daily", "weekly", "monthly" };
        public List<string> updateIntervalsList = new List<string> { "2sec", "5sec", "10sec", "30sec", "1min", "5min" };

        public MainWindow()
        {
            //var chart = new LineChart();
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            
            this.DataContext = this;

            digitalCurrenciesList = parser.parse("digital_currencies.txt");
            physicalCurrenciesList = parser.parse("physical_currencies.txt");
            stocksList = parser.parse("stocks.txt");

            chosenDigitalList = new ObservableCollection<CurrencyClass>();
            chosenPhysicalList = new ObservableCollection<CurrencyClass>();
            chosenStockList = new ObservableCollection<CurrencyClass>();

            referentCurrenciesList = physicalCurrenciesList;
            shownCurrenciesList = new ObservableCollection<ShowingCurrencyClass>();
            referentCurrency = new CurrencyClass();

            loadData();
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


        private void ComboBox_Loaded_Refs(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = data2ListStrings(referentCurrenciesList);
        }

        private void ComboBox_Loaded_Digital(object sender, RoutedEventArgs e)
        {


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = digitalCurrenciesList;

            // ... Make the first item selected.
        }





        private void ComboBox_Loaded_Physical(object sender, RoutedEventArgs e)
        {


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = physicalCurrenciesList;

            // ... Make the first item selected.
        }

        private void ComboBox_Loaded_Stocks(object sender, RoutedEventArgs e)
        {


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = stocksList;

            // ... Make the first item selected.
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
                    //i da se updatuje na svakih 5 sec

                    c.Client = new LoadJSON();
                        
                    c.startTiming(updateInterval);
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
                    c.startTiming(updateInterval);

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
            string symbol = ((CurrencyClass)dataGridDigital.SelectedItem).Symbol;
            chosenDigitalList.Remove((CurrencyClass)dataGridDigital.SelectedItem);

            foreach (CurrencyClass c in digitalCurrenciesList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    c.CheckedBox = false;
                    break;
                }
            }

        }

        private void removePhysicalButton(Object sender, RoutedEventArgs e)
        {
            string symbol = ((CurrencyClass)dataGridPhysical.SelectedItem).Symbol;
            chosenPhysicalList.Remove((CurrencyClass)dataGridPhysical.SelectedItem);

            foreach (CurrencyClass c in physicalCurrenciesList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    c.CheckedBox = false;
                    break;
                }
            }

        }

        private void removeStockButton(Object sender, RoutedEventArgs e)
        {
            string symbol = ((CurrencyClass)dataGridStock.SelectedItem).Symbol;
            chosenStockList.Remove((CurrencyClass)dataGridStock.SelectedItem);

            foreach (CurrencyClass c in stocksList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    c.CheckedBox = false;
                    break;
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
                       
                        CurrencyIntervalType type;
                        switch (cbHistoryInterval.SelectedItem.ToString())
                        {
                            case "intraday":
                                type = CurrencyIntervalType.DIGITAL_CURRENCY_INTRADAY;
                                break;
                            case "daily":
                                type = CurrencyIntervalType.DIGITAL_CURRENCY_DAILY;
                                break;
                            case "weekly":
                                type = CurrencyIntervalType.DIGITAL_CURRENCY_WEEKLY;
                                break;
                            default:
                                type = CurrencyIntervalType.DIGITAL_CURRENCY_MONTHLY;
                                break;
                        }

                        loadJSON(type, symbol);
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

                       
                        CurrencyIntervalType type;
                        switch (cbHistoryInterval.SelectedItem.ToString())
                        {
                            case "intraday":
                                type = CurrencyIntervalType.TIME_SERIES_INTRADAY;
                                break;
                            case "daily":
                                type = CurrencyIntervalType.TIME_SERIES_DAILY;
                                break;
                            case "weekly":
                                type = CurrencyIntervalType.TIME_SERIES_WEEKLY;
                                break;
                            default:
                                type = CurrencyIntervalType.TIME_SERIES_MONTHLY;
                                break;
                        }

                        loadJSON(type, symbol);
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
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_INTRADAY&symbol="+symbol+ "&market="+referentCurrency.Symbol+"&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Intraday)";
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_DAILY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_DAILY&symbol="+symbol+ "&market=" + referentCurrency.Symbol + "&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Daily)";
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_WEEKLY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_WEEKLY&symbol="+symbol+ "&market=" + referentCurrency.Symbol + "&apikey=9P2LP0T1YR34LBSK";
                    keyForTimeseries = "Time Series (Digital Currency Weekly)";
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_MONTHLY:
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_MONTHLY&symbol="+symbol+ "&market=" + referentCurrency.Symbol + "&apikey=9P2LP0T1YR34LBSK";
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

            if (!response.Contains("Error") && !response.Contains("kindly contact support") && !string.IsNullOrEmpty(response))
            {
                var jObj = JsonConvert.DeserializeObject<dynamic>(response);
                ShowingCurrencyClass showingCurrency = new ShowingCurrencyClass();

                showingCurrency.Metadata = jObj["Meta Data"].ToObject<Dictionary<string, string>>();
                showingCurrency.Type = interval;

                //u zavisnosti od intervala se prosledjuje kljuc za timeseries
                showingCurrency.Timeseries = jObj[keyForTimeseries].ToObject<Dictionary<string, Dictionary<string, double>>>();

                shownCurrenciesList.Add(showingCurrency);

                addNewTab(showingCurrency);
            }else
            {
                MessageBox.Show("unable to collect data");
            }

            
        }

        private void addNewTab(ShowingCurrencyClass showingCurrency)
        {
            TabItem tabItem = new TabItem();
            CartesianChart cart = new CartesianChart();

            ChartValues<double> vals = new ChartValues<double>();
            List<string> labels = new List<string>();

            string title = "";
            string time = "";

            switch (showingCurrency.Type)
            {
                case CurrencyIntervalType.TIME_SERIES_INTRADAY:
                case CurrencyIntervalType.TIME_SERIES_DAILY:
                case CurrencyIntervalType.TIME_SERIES_WEEKLY:
                case CurrencyIntervalType.TIME_SERIES_MONTHLY:
                    string[] lista = showingCurrency.Metadata["1. Information"].Split(' ');
                    if (showingCurrency.Type == CurrencyIntervalType.TIME_SERIES_INTRADAY)
                    {
                        time = lista[0] + lista[1];
                    }
                    else
                    {
                        time = lista[0];
                    }
                    title = showingCurrency.Metadata["2. Symbol"] + "("+time+")";
                    foreach (string key in showingCurrency.Timeseries.Keys)
                    {
                        labels.Add(key);
                        labels.Add("");
                        labels.Add("");
                        labels.Add("");
                        vals.Add(showingCurrency.Timeseries[key]["1. open"]);
                        vals.Add(showingCurrency.Timeseries[key]["2. high"]);
                        vals.Add(showingCurrency.Timeseries[key]["3. low"]);
                        vals.Add(showingCurrency.Timeseries[key]["4. close"]);
                    }
                    break;
                case CurrencyIntervalType.DIGITAL_CURRENCY_INTRADAY:
                case CurrencyIntervalType.DIGITAL_CURRENCY_DAILY:
                case CurrencyIntervalType.DIGITAL_CURRENCY_MONTHLY:
                case CurrencyIntervalType.DIGITAL_CURRENCY_WEEKLY:
                    string[] listing = showingCurrency.Metadata["1. Information"].Split(' ');
                    if (showingCurrency.Type == CurrencyIntervalType.DIGITAL_CURRENCY_INTRADAY)
                    {
                        time = listing[0] + listing[1];
                    }
                    else
                    {
                        time = listing[0];
                    }
                    title = showingCurrency.Metadata["2. Symbol"] + "(" + time+")";
                    foreach (string key in showingCurrency.Timeseries.Keys)
                    {
                        labels.Add(key);
                        labels.Add("");
                        labels.Add("");
                        labels.Add("");
                        vals.Add(showingCurrency.Timeseries[key]["1a. open ("+referentCurrency.Symbol+")"]);
                        vals.Add(showingCurrency.Timeseries[key]["2a. high (" + referentCurrency.Symbol + ")"]);
                        vals.Add(showingCurrency.Timeseries[key]["3a. low (" + referentCurrency.Symbol + ")"]);
                        vals.Add(showingCurrency.Timeseries[key]["4a. close (" + referentCurrency.Symbol + ")"]);
                    }
                    break;
            }

            LiveCharts.SeriesCollection sers = new LiveCharts.SeriesCollection
                {
                new LineSeries
                {
                    Title = title,
                    Values = vals                    
                }
            };
            //cart.LegendLocation = LiveCharts.LegendLocation.Right;
            cart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = labels,
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 4
                },
                LabelsRotation = 15
            });

            cart.Series = sers;
            cart.Width = vals.Count*10;
            
            ScrollViewer skrol = new ScrollViewer();
            skrol.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;

            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "Close";
            
            mi.Click += new RoutedEventHandler((sender, e) => menuItemClicked(sender,e, title));
            cm.Items.Add(mi);

            tabItem.ContextMenu = cm;
            tabItem.Header = title;
            skrol.Content = cart;
            tabItem.Content = skrol;
            tabControl.Items.Add(tabItem);
            tabControl.SelectedItem = tabItem;
        }

        void menuItemClicked(object sender, RoutedEventArgs e, string title )
        {
            foreach(TabItem ti in tabControl.Items)
            {
                if (ti.Header.Equals(title))
                {
                    tabControl.Items.Remove(ti);
                    break;
                }
            }

            string time, compare;
            string[] lista = null;
            foreach(ShowingCurrencyClass scc in shownCurrenciesList)
            {
                lista = scc.Metadata["1. Information"].Split(' ');
                if (scc.Type == CurrencyIntervalType.TIME_SERIES_INTRADAY || scc.Type==CurrencyIntervalType.DIGITAL_CURRENCY_INTRADAY)
                {
                    time = lista[0] + lista[1];
                }
                else
                {
                    time = lista[0];
                }
                compare = scc.Metadata["2. Symbol"] + "(" + time + ")";
                if (compare.Equals(title))
                {
                    shownCurrenciesList.Remove(scc);
                    break;
                }
            }

            
            MessageBox.Show("Removed" + title);
        }


        private void cbRefCurrencies_Selected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string name = comboBox.SelectedItem.ToString();
            string symbol = name.Split('(')[1].Split(')')[0];
            foreach (CurrencyClass c in referentCurrenciesList)
            {
                if (c.Symbol.Equals(symbol))
                {
                    referentCurrency = c;
                    break;
                }
            }

            foreach (CurrencyClass cur in chosenDigitalList)
            {
                cur.startTiming(5);
            }
            foreach (CurrencyClass cur in chosenPhysicalList)
            {
                cur.startTiming(5);
            }
        }

        private void loadData()
        {
            using (StreamReader cit = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\files\\savings.txt"))
            {
                string line = cit.ReadLine();
                string[] lista = line.Split(',');

                referentCurrency.Symbol = lista[0];
                referentCurrency.Name = lista[1];
                string indexRef = lista[2];

                cit.ReadLine();
                while((line=cit.ReadLine())!= "ChosenPhysical")
                {
                    lista = line.Split(',');
                    CurrencyClass cc = new CurrencyClass();
                    cc.Symbol = lista[0];
                    cc.Name = lista[1];

                    cc.Client = new LoadJSON();
                    cc.startTiming(5);

                    foreach (CurrencyClass cur in digitalCurrenciesList)
                    {
                        if (cur.Symbol.Equals(cc.Symbol))
                        {
                            cur.CheckedBox = true;
                        }
                    }

                    chosenDigitalList.Insert(0, cc);
                }
                while ((line = cit.ReadLine()) != "ChosenStock")
                {
                    lista = line.Split(',');
                    CurrencyClass cc = new CurrencyClass();
                    cc.Symbol = lista[0];
                    cc.Name = lista[1];
                    cc.Client = new LoadJSON();
                    cc.startTiming(5);                    

                    foreach (CurrencyClass cur in physicalCurrenciesList)
                    {
                        if (cur.Symbol.Equals(cc.Symbol)){
                            cur.CheckedBox = true;
                        }
                    }

                    chosenPhysicalList.Insert(0, cc);
                }
                while ((line = cit.ReadLine()) != "Shown")
                {
                    lista = line.Split(',');
                    CurrencyClass cc = new CurrencyClass();
                    cc.Symbol = lista[0];
                    cc.Name = lista[1];

                    foreach (CurrencyClass cur in stocksList)
                    {
                        if (cur.Symbol.Equals(cc.Symbol))
                        {
                            cur.CheckedBox = true;
                        }
                    }

                    chosenStockList.Insert(0, cc);
                }
                while ((line = cit.ReadLine()) != null)
                {
                    lista = line.Split(',');
                    object o = Enum.Parse(typeof(CurrencyIntervalType), lista[1]);
                    loadJSON((CurrencyIntervalType)o, lista[0]);
                }
                cbRefCurrencies.SelectedIndex = int.Parse(indexRef);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            using(StreamWriter pis = new StreamWriter(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\files\\savings.txt" ))
            {
                pis.WriteLine(referentCurrency.Symbol + "," + referentCurrency.Name+","+ cbRefCurrencies.SelectedIndex);
                pis.WriteLine("ChosenDigital");
                foreach(CurrencyClass cc in chosenDigitalList)
                {
                    pis.WriteLine(cc.Symbol + "," + cc.Name);
                }
                pis.WriteLine("ChosenPhysical");
                foreach(CurrencyClass cc in chosenPhysicalList)
                {
                    pis.WriteLine(cc.Symbol + "," + cc.Name);
                }
                pis.WriteLine("ChosenStock");
                foreach(CurrencyClass cc in chosenStockList)
                {
                    pis.WriteLine(cc.Symbol + "," + cc.Name);
                }
                pis.WriteLine("Shown");
                foreach(ShowingCurrencyClass scc in shownCurrenciesList)
                {
                    pis.WriteLine(scc.Metadata.ElementAt(1).Value+","+scc.Type);
                }
            }
        }

        private void cbUpdateInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = comboBox.SelectedItem.ToString();
            updateInterval = Convert.ToDouble(item.Substring(0, item.Length - 3));
        }

        private void ComboBox_Loaded_History(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.ItemsSource = historyIntervalsList;
            comboBox.SelectedIndex = 0;
        }

        private void ComboBox_Loaded_Update(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.ItemsSource = updateIntervalsList;
            comboBox.SelectedIndex = 0;
        }
    }
}
