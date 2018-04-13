using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrviProj
{
    public enum CurrencyType
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
        private CurrencyType _type; 

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


        public CurrencyType Type
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


        public string toString()
        {
            return Name + " " + Symbol;
        }
        
    }
}
