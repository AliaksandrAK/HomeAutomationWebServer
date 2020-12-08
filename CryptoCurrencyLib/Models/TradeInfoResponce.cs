using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoCurrency.Models
{
    public class TradeInfoResponce
    {
        public string instrument { get; set; }
        public decimal? last { get; set; }
        public decimal? percentChange { get; set; }
        public decimal? low { get; set; }
        public decimal? high { get; set; }
        public decimal baseVolume { get; set; }
        public decimal quoteVolume { get; set; }
        public decimal volumeInBtc { get; set; }
        public decimal volumeInUsd { get; set; }
        public decimal? ask { get; set; }
        public decimal? bid { get; set; }
        public DateTime timestamp { get; set; }
    }
}