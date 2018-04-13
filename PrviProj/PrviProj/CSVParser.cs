using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using System.IO;

namespace PrviProj
{
    class CSVParser
    {

        public ObservableCollection<CurrencyClass> parse(string path)
        {
            ObservableCollection<CurrencyClass> currencyList = new ObservableCollection<CurrencyClass>();
            path=Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\files\\" + path;
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    
                    string[] fields = parser.ReadFields();
                    CurrencyType type = CurrencyType.TIME_SERIES_DAILY;
                    if (path.Equals("digital_currencies.txt"))
                    {
                        type = CurrencyType.DIGITAL_CURRENCY_DAILY;
                    }
                    currencyList.Add(new CurrencyClass { Symbol = fields[0], Name = fields[1], Type = type});
                    
                }
            }

            return currencyList;
        }

    }
}
