using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoCurrency.Models
{
    public class CryptoRecommendation
    {
        public string CryptoPath { get; set; }
        public decimal VolumeIn { get; set; }
        public decimal PrevOut { get; set; }
        public decimal VolumeOut { get; set; }
        public decimal VolumeUsd { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}