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
                string link = string.Empty;
                string keyForTimeseries = string.Empty;
                while (!parser.EndOfData)
                {
                    //Processing row
                    
                    string[] fields = parser.ReadFields();

                    keyForTimeseries = "Time Series (Digital Currency Monthly)";
                    link = "https://www.alphavantage.co/query?function=DIGITAL_CURRENCY_WEEKLY&symbol=" + fields[0] + "&market=CNY&apikey=9P2LP0T1YR34LBSK";
                    try
                    {
                        LoadJSON client = new LoadJSON();
                        client.endPoint = link;

                        string response = string.Empty;
                        //response = client.makeRequest();
                        currencyList.Add(new CurrencyClass { Symbol = fields[0], Name = fields[1] });

                    }
                    catch (Exception e) { }
                    
                }
            }

            return currencyList;
        }

    }
}
