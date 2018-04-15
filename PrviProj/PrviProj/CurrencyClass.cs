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
        private string _value;
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


        public string Value
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


        public TimeSpan Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (value != _interval)
                {
                    _interval = value;
                    OnPropertyChanged("Interval");
                }
            }
        }


        public void startTiming(int seconds)
        {
            this.Value = "0";
            this.Interval = TimeSpan.FromSeconds(seconds);
            this.Timer = new Timer((e) =>
            {
                getExchangeRate();
            }, null, _begin, _interval);
           
                     

        }

        private void getExchangeRate()
        {
            //vuku se podaci sa neta za zadat simbol i referentnu valutu
            string refSymbol = "USD";
            string link = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency="+this.Symbol+"&to_currency="+refSymbol+"&apikey=9P2LP0T1YR34LBSK";

            this.Client.endPoint = link;

            string response = string.Empty;
            response = this.Client.makeRequest();           

            var jObj = JsonConvert.DeserializeObject<dynamic>(response);
            try
            {
                Dictionary<string, string> recnik = jObj["Realtime Currency Exchange Rate"].ToObject<Dictionary<string, string>>();

                this.Value = recnik["5. Exchange Rate"];
            }catch
            {
                if (this.Value.Equals("0"))
                {
                    this.Value = "unable to fetch";
                }
                
            }
            
        }


        public string toString()
        {
            return Name + " " + Symbol;
        }
        
    }
}
