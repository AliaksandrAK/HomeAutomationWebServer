using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace CryptoCurrency
{
    public class CryptoInfoService
    {
        Dictionary<int, List<string>> _currencyPath;
        Dictionary<int, List<string>> _currencyPairs;
        List<Models.TradeInfo> _tradesInfoList;
        List<Models.TradeInfo> _tradesProccedList;
        List<Models.CurrencyPairInfo> _currencyPairInfo;
        string _xrp = "XRP";
        string _ltc = "LTC";
        string _btc = "BTC";
        string _eth = "ETH";
        string _eos = "EOS";
        string _etc = "ETC";
        string _btg = "BTG";

        
        decimal _xrpBtc = 0.00002882m;
        decimal _xrpBtcRec = 0.000032m;
        decimal _xrpBtcSell = 0.00003215m;
        bool _kupilXrp = true;//true = купил XRP за BTC

        decimal _ltcEth = 0;
        decimal _ltcEthRec = 0.13m;
        decimal _ltcEthSell = 0.1501m;
        bool _kupilLtcEth = false;//true = купил LTC за ETH

        decimal _eosBtc = 0;
        decimal _eosBtcRec = 0.00016m;
        decimal _eosBtcSell = 0;
        bool _kupilEosBtc = true;//true = купил EOS за BTC

        decimal _btgBtc = 0;
        decimal _btgBtcRec = 0.000495m;
        decimal _btgBtcSell = 0;
        bool _kupilBtgBtc = true;//true = купил EOS за BTC

        void AddWorkPairs()
        {
            Models.CurrencyPairInfo curitem = new Models.CurrencyPairInfo();
            curitem.FirstSymbol = _xrp;
            curitem.SecondSymbol = _btc;
            curitem.BuyRate = _xrpBtc;
            curitem.SellRate = _xrpBtcSell;
            curitem.RecomRate = _xrpBtcRec;
            curitem.BuySellMode = _kupilXrp;
            _currencyPairInfo.Add(curitem);

            Models.CurrencyPairInfo curitem1 = new Models.CurrencyPairInfo();
            curitem1.FirstSymbol = _ltc;
            curitem1.SecondSymbol = _eth;
            curitem1.BuyRate = _ltcEth;
            curitem1.SellRate = _ltcEthSell;
            curitem1.RecomRate = _ltcEthRec;
            curitem1.BuySellMode = _kupilLtcEth;
            _currencyPairInfo.Add(curitem1);

            Models.CurrencyPairInfo curitem2 = new Models.CurrencyPairInfo();
            curitem2.FirstSymbol = _eos;
            curitem2.SecondSymbol = _btc;
            curitem2.BuyRate = _eosBtc;
            curitem2.SellRate = _eosBtcSell;
            curitem2.RecomRate = _eosBtcRec;
            curitem2.BuySellMode = _kupilEosBtc;
            _currencyPairInfo.Add(curitem2);

            Models.CurrencyPairInfo curitem3 = new Models.CurrencyPairInfo();
            curitem3.FirstSymbol = _btg;
            curitem3.SecondSymbol = _btc;
            curitem3.BuyRate = _btgBtc;
            curitem3.SellRate = _btgBtcSell;
            curitem3.RecomRate = _btgBtcRec;
            curitem3.BuySellMode = _kupilBtgBtc;
            _currencyPairInfo.Add(curitem3);
        }
        void AddPairs()
        {
            _currencyPairs = new Dictionary<int, List<string>>();
            int iKey = 1;
            foreach (Models.CurrencyPairInfo curitem in _currencyPairInfo)
            {
                List<string> itemPair1 = new List<string> { curitem.FirstSymbol, curitem.SecondSymbol };
                _currencyPairs.Add(iKey, itemPair1);
                iKey++;
            }
        }
        void AddRecommendationInfo()
        {
            foreach(Models.CurrencyPairInfo curitem in _currencyPairInfo)
            {
                _tradesProccedList.Add(AddItem(curitem.BuyRate, curitem.SellRate, curitem.RecomRate,
                                        curitem.BuySellMode, curitem.FirstSymbol, curitem.SecondSymbol,DateTime.Now));
            }
        }

        Models.TradeInfo AddItem(decimal curBuy, decimal curSell, decimal curRecom, bool kupil,
                        string symbol1, string symbol2, DateTime procDate)
        {
            Models.TradeInfo newItem = new Models.TradeInfo();
            newItem.Symbol = symbol1 + "/" + symbol2;
            newItem.PurchaseRate = curBuy;
            newItem.SellingRate = curSell;
            newItem.UpdatedDate = procDate;
            string tmpCoinChange = "Продал " + symbol1 + " за " + curSell.ToString();
            newItem.Comment = tmpCoinChange + ". Рекоммендация купить " + symbol1 + " за " + curRecom.ToString();
            if (kupil)
            {
                tmpCoinChange = "Купил " + symbol1 + " за " + curBuy.ToString();
                newItem.Comment = tmpCoinChange + ". Рекоммендация продать " + symbol1 + " за " + curRecom.ToString();
            }

            return newItem;
        }
        string GetColor(decimal rate, bool kupil, decimal curRecom)
        {
            string sColor = "red";
            if (kupil)
            {
                if (rate > curRecom) sColor = "green";
            }
            else if (rate < curRecom) sColor = "green";

            return sColor;
        }

        public CryptoInfoService()
        {
            _currencyPath = new Dictionary<int, List<string>>();
            List<string> itemPath = new List<string> { _ltc, _btc, _eth, _ltc }; //LTC - BTC - ETH - LTC
            _currencyPath.Add(1, itemPath);
            //List<string> itemPath1 = new List<string> { "LTC", "BTC", "ETC", "ETH", "LTC" }; //LTC-BTC-ETC-ETH-LTC
            //_currencyPath.Add(2, itemPath1);
            List<string> itemPath2 = new List<string> { _ltc, _eth, _btc, _ltc };
            _currencyPath.Add(3, itemPath2);
            //List<string> itemPath3 = new List<string> { "ETC", "ETH", "BTC", "ETC" };
            //_currencyPath.Add(4, itemPath3);
            //List<string> itemPath4 = new List<string> { "ETC", "BTC", "ETH", "ETC" };
            //_currencyPath.Add(5, itemPath4);

            List<string> itemPath9 = new List<string> { _ltc, _btc, _etc };
            _currencyPath.Add(10, itemPath9);
            List<string> itemPath10 = new List<string> { _ltc, _eth, _etc };
            _currencyPath.Add(11, itemPath10);
            //List<string> itemPath11 = new List<string> { "ETC", "BTC", "LTC" };
            //_currencyPath.Add(12, itemPath11);
            //List<string> itemPath12 = new List<string> { "ETC", "ETH", "LTC" };
            //_currencyPath.Add(13, itemPath12);

            //List<string> itemPath5 = new List<string> { "ETC", "BTC", "USD" };
            //_currencyPath.Add(6, itemPath5);
            //List<string> itemPath6 = new List<string> { "ETC", "ETH", "USD" };
            //_currencyPath.Add(7, itemPath6);
            List<string> itemPath7 = new List<string> { _ltc, _btc, "USDPM" };
            _currencyPath.Add(8, itemPath7);
            List<string> itemPath8 = new List<string> { _ltc, _eth, "USDPM" };
            _currencyPath.Add(9, itemPath8);

            List<string> itemPath20 = new List<string> { _xrp, _btc, "USDPM" };
            _currencyPath.Add(20, itemPath20);
            List<string> itemPath23 = new List<string> { _xrp, _eth, "USDPM" };
            _currencyPath.Add(23, itemPath23);
            List<string> itemPath21 = new List<string> { _eos, _btc, "USDPM" };
            _currencyPath.Add(21, itemPath21);
            List<string> itemPath22 = new List<string> { _btg, _btc, "USDPM" };
            _currencyPath.Add(22, itemPath22);

            /*
            List<string> itemPath13 = new List<string> { "ETC", "BTC", "USD", "ETH", "ETC" };
            _currencyPath.Add(14, itemPath13);
            List<string> itemPath14 = new List<string> { "ETC", "ETH", "USD", "BTC", "ETC" };
            _currencyPath.Add(15, itemPath14);
            List<string> itemPath15 = new List<string> { "LTC", "BTC", "USD", "ETH", "LTC" };
            _currencyPath.Add(16, itemPath15);
            List<string> itemPath16 = new List<string> { "LTC", "ETH", "USD", "BTC", "LTC" };
            _currencyPath.Add(17, itemPath16);
            */

            //
            _tradesInfoList = new List<Models.TradeInfo>();
            _tradesProccedList = new List<Models.TradeInfo>();
            _currencyPairInfo = new List<Models.CurrencyPairInfo>();

            AddWorkPairs();
            AddPairs();
            AddRecommendationInfo();
        }
        public List<Models.TradeInfo> GetCurrenciesInfo()
        {
            List<Models.TradeInfo> result = new List<Models.TradeInfo>();

            string tempUrl = "https://api.crex24.com/v2/public/tickers";
            try
            {
                WebRequest request = WebRequest.Create(tempUrl);
                request.Method = "GET";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string json = reader.ReadToEnd();
                List<Models.TradeInfoResponce> tradesInfo = JsonConvert.DeserializeObject<List<Models.TradeInfoResponce>>(json);

                foreach (var tradeItem in tradesInfo)
                {
                    if (tradeItem.instrument.Contains("-USD") && tradeItem.bid.HasValue && tradeItem.ask.HasValue)
                    {
                        Models.TradeInfo newItem = new Models.TradeInfo();
                        newItem.Symbol = tradeItem.instrument.Replace("-USD", "");
                        newItem.PurchaseRate = tradeItem.ask.Value;
                        newItem.SellingRate = tradeItem.bid.Value;
                        newItem.UpdatedDate = tradeItem.timestamp;
                        result.Add(newItem);
                    }
                }
            }
            catch (WebException)
            {

            }

            return result.OrderByDescending(rp => rp.SellingRate).ToList();
        }
        public List<Models.CryptoRecommendation> GetRecommendationInfo()
        {
            List<Models.CryptoRecommendation> result = new List<Models.CryptoRecommendation>();
            try
            {
                List<Models.TradeInfo> tradesInfoList = GetPairsInfo();
                //calculate recommendation
                foreach (var pathItem in _currencyPath)
                {
                    decimal outPrice = 1;//start value for example 1 LTC if first currency in the path is LTC
                    Models.CryptoRecommendation itemPath = new Models.CryptoRecommendation();
                    itemPath.CryptoPath = string.Join("-", pathItem.Value);
                    itemPath.VolumeIn = outPrice;
                    itemPath.UpdatedDate = DateTime.Now;

                    for (int i = 0; i < (pathItem.Value.Count - 1); i++)
                    {
                        string pair1 = pathItem.Value[i] + "-" + pathItem.Value[i + 1];
                        string pair2 = pathItem.Value[i + 1] + "-" + pathItem.Value[i];
                        foreach (var pair in tradesInfoList)
                        {
                            if (pair.Symbol.Equals(pair1))
                            {
                                if (i == (pathItem.Value.Count - 2)) itemPath.PrevOut = pair.SellingRate;
                                outPrice = outPrice * pair.SellingRate; break;
                            }
                            else if (pair.Symbol.Equals(pair2))
                            {
                                if (i == (pathItem.Value.Count - 2)) itemPath.PrevOut = pair.PurchaseRate;
                                outPrice = outPrice / pair.PurchaseRate; break;
                            }
                        }
                    }
                    itemPath.VolumeOut = outPrice;
                    result.Add(itemPath);
                }
            }
            catch (WebException)
            {

            }

            return result;
        }
        public List<Models.TradeInfo> GetPairsInfo()
        {
            if (_tradesInfoList.Count > 0) return _tradesInfoList;
            string tempUrl = "https://api.crex24.com/v2/public/tickers";
            try
            {
                WebRequest request = WebRequest.Create(tempUrl);
                request.Method = "GET";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string json = reader.ReadToEnd();
                List<Models.TradeInfoResponce> tradesInfo = JsonConvert.DeserializeObject<List<Models.TradeInfoResponce>>(json);

                //find all pairs
                foreach (var tradeItem in tradesInfo)
                {
                    if (tradeItem.ask.HasValue && tradeItem.bid.HasValue)
                    /*
                    if (tradeItem.instrument.Length ==7 &&
                        (tradeItem.instrument.Contains("LTC") && tradeItem.instrument.Contains("BTC") ||
                        tradeItem.instrument.Contains("BTC") && tradeItem.instrument.Contains("ETC") ||
                        tradeItem.instrument.Contains("ETC") && tradeItem.instrument.Contains("ETH") ||
                        tradeItem.instrument.Contains("ETH") && tradeItem.instrument.Contains("LTC") ||
                        tradeItem.instrument.Contains("BTC") && tradeItem.instrument.Contains("ETH"))
                        )
                    */
                    {
                        Models.TradeInfo newItem = new Models.TradeInfo();
                        newItem.Symbol = tradeItem.instrument;
                        newItem.PurchaseRate = tradeItem.ask.Value;
                        newItem.SellingRate = tradeItem.bid.Value;
                        newItem.UpdatedDate = tradeItem.timestamp;

                        _tradesInfoList.Add(newItem);
                    }
                }
            }
            catch (WebException)
            {

            }

            return _tradesInfoList;
        }
        public List<Models.TradeInfo> GetPairsProcessed()
        {
            return _tradesProccedList;
        }
        public List<Models.TradeInfo> GetMyPairsInfo(out bool Alert)
        {
            Alert = false;
            List<Models.TradeInfo> tradesInfoList = GetPairsInfo();
            List<Models.TradeInfo> resultList = new List<Models.TradeInfo>();
            foreach (var pair in tradesInfoList)
            {
                foreach (var pathItem in _currencyPairs)
                {
                    for (int i = 0; i < (pathItem.Value.Count - 1); i++)
                    {
                        string pair1 = pathItem.Value[i] + "-" + pathItem.Value[i + 1];
                        string pair2 = pathItem.Value[i + 1] + "-" + pathItem.Value[i];
                        if (pair.Symbol.Equals(pair1) || pair.Symbol.Equals(pair2))
                        {
                            pair.ColorRecom = "";
                            pair.ColorRecom1 = "";
                            decimal dRate = pair.PurchaseRate;
                            foreach (Models.CurrencyPairInfo curitem in _currencyPairInfo)
                            {
                                if (pair.Symbol.Contains(curitem.FirstSymbol) && pair.Symbol.Contains(curitem.SecondSymbol))
                                {
                                    if (curitem.BuySellMode) dRate = pair.SellingRate;
                                    pair.Color = GetColor(dRate, curitem.BuySellMode, curitem.RecomRate);
                                    pair.RecommendRate = curitem.RecomRate;
                                    if (curitem.BuySellMode) pair.ColorRecom = pair.Color;
                                    else pair.ColorRecom1 = pair.Color;

                                    break;
                                }
                            }                            
                            if (pair.Color.Contains("green")) Alert = true;
                            resultList.Add(pair);
                        }
                    }
                }
            }
            return resultList;
        }

        public void CheckRecomendation(List<Models.TradeInfo> pairs, List<Order> orders)
        {
            foreach (var pair in pairs)
            {
                var last = orders.FirstOrDefault();
            }
        }
        public string GetInfoJson(List<Models.TradeInfo> tradeInfo)
        {
            List<Models.TradeInfo> tmp = new List<Models.TradeInfo>();
            foreach(Models.TradeInfo item in tradeInfo)
            {
                if (item.Color.Contains("green")) tmp.Add(item);
            }
            return JsonConvert.SerializeObject(tmp);
        }

        public void UpdateWorkCurrencyPairs(List<Models.CurrencyPairInfo> pairs)
        {
            _currencyPairInfo.Clear();
            _tradesProccedList.Clear();
            _currencyPairs.Clear();

            _currencyPairInfo.AddRange(pairs);
            AddPairs();
            AddRecommendationInfo();
        }
    }
}