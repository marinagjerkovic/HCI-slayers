using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public string toString()
        {
            return Name + " " + Symbol;
        }
        
    }
}
