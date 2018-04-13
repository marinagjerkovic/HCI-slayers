using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrviProj
{
    class Currency
    {
       
        public int Broj { get; set; }
        public string Ime { get; set; }
        public int Broj2 { get; set; }
        public string Ime2 { get; set; }
        public Currency(int br, string i, int br2, string i2)
        {
            Ime2 = i2;
            Broj = br;
            Ime = i;
            Broj2 = br2;
        }
        public static ObservableCollection<Currency> generateData()
        {
            ObservableCollection<Currency> currencies = new ObservableCollection<Currency>();
            Currency p1 = new Currency(1, "a", 11,"aa");
            Currency p2 = new Currency(2, "b", 22,"bb");
            Currency p3 = new Currency(3, "c", 33,"cc");
            Currency p4 = new Currency(4, "d", 44, "dd");

            currencies.Add(p1); currencies.Add(p2); currencies.Add(p3); currencies.Add(p4);
            return currencies;
        }
    }
}
