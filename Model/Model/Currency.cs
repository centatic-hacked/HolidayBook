using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Currency
    {
        protected Currency()
        { }
        public Currency(string currency_Code, string currency_Symbol)
        {
            Currency_Code = currency_Code;
            Currency_Symbol = currency_Symbol;
        }

        public string Currency_Code { get; private set; }

        public string Currency_Symbol { get; set; }
    }
}
