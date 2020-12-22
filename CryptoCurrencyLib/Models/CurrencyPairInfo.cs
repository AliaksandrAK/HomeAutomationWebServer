using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Models
{
    public class CurrencyPairInfo
    {
        public string FirstSymbol { get; set; }
        public string SecondSymbol { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public decimal RecomRate { get; set; }
        public bool BuySellMode { get; set; }
    }
}
