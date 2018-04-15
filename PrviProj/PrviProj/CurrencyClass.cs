using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PrviProj
{
    

    public class CurrencyClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private string _name;
        private string _symbol;
        private double _value;
        private Timer _timer;
        private LoadJSON _client;
        private TimeSpan _begin = TimeSpan.Zero;
        private TimeSpan _interval; //interval na koji ce se updatovati vrednost valute

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (value != _symbol)
                {
                    _symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
        }


        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public Timer Timer
        {
            get
            {
                return _timer;
            }
            set
            {
                if (value != _timer)
                {
                    _timer = value;
                    OnPropertyChanged("Timer");
                }
            }
        }


        public LoadJSON Client
        {
            get
            {
                return _client;
            }
            set
            {
                if (value != _client)
                {
                    _client = value;
                    OnPropertyChanged("Client");
                }
            }
        }


        public void startTiming(int seconds)
        {
            _client = new LoadJSON();
            _interval = TimeSpan.FromSeconds(seconds);
            _timer = new Timer((e) =>
            {
                getExchangeRate();
            }, null, _begin, _interval);
           
                     

        }

        private void getExchangeRate()
        {
            //vuku se podaci sa neta za zadat simbol i referentnu valutu
            string refSymbol = "USD";
            string link = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="+_symbol+"&to_currency="+refSymbol+"&apikey=9P2LP0T1YR34LBSK";

            _client.endPoint = link;

            string response = string.Empty;
            response = _client.makeRequest();
            if (!response.Contains("Information"))
            {
                MessageBox.Show(response);
                var jObj = JsonConvert.DeserializeObject<dynamic>(response);
                Dictionary<string, string> recnik = jObj["Realtime Currency Exchange Rate"].ToObject<Dictionary<string, string>>();

                this.Value = Convert.ToDouble(recnik["5. Exchange Rate"]);
            }
            else
            {
                MessageBox.Show(response);

            }
        }


        public string toString()
        {
            return Name + " " + Symbol;
        }
        
    }
}
