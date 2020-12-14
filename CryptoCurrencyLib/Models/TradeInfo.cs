using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoCurrency.Models
{
    public class TradeInfo

    {
        public string Symbol { get; set; }
        public decimal SellingRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string Comment { get; set; }

        public string Color { get; set; }

        public decimal RecommendRate { get; set; }
        public string ColorRecom { get; set; }
        public string ColorRecom1 { get; set; }

    }
}