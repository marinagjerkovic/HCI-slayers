using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrviProj
{
    public enum CurrencyIntervalType
    {
        TIME_SERIES_INTRADAY,
        TIME_SERIES_DAILY,
        TIME_SERIES_WEEKLY,
        TIME_SERIES_MONTHLY,
        DIGITAL_CURRENCY_INTRADAY,
        DIGITAL_CURRENCY_DAILY,
        DIGITAL_CURRENCY_WEEKLY,
        DIGITAL_CURRENCY_MONTHLY
    }

    public class ShowingCurrencyClass : INotifyPropertyChanged 
    {        


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        private Dictionary<string, string> _metadata;
        private Dictionary<string, Dictionary<string, string>> _timeseries;
        CurrencyIntervalType _type;

        public Dictionary<string, string> Metadata
        {
            get
            {
                return _metadata;
            }
            set
            {
                if (value != _metadata)
                {
                    _metadata = value;
                    OnPropertyChanged("Metadata");
                }
            }
        }


        public Dictionary<string, Dictionary<string, string>> Timeseries
        {
            get
            {
                return _timeseries;
            }
            set
            {
                if (value != _timeseries)
                {
                    _timeseries = value;
                    OnPropertyChanged("Timeseries");
                }
            }
        }

        public CurrencyIntervalType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

    }
}
