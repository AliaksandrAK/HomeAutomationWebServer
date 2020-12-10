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
        string _xrp = "XRP";
        string _ltc = "LTC";
        string _btc = "BTC";
        string _eth = "ETH";
        string _eos = "EOS";
        string _etc = "ETC";

        decimal _xrpBtc = 0.00002871m;
        decimal _xrpBtcRec = 0.000032m;
        bool _kupilXrp = true;//true = купил XRP за BTC

        decimal _ltcBtc = 0;
        decimal _ltcBtcRec = 0.0044m;
        bool _kupilLtc = true;//true = купил LTC за BTC

        decimal _ltcEth = 0;
        decimal _ltcEthRec = 0.15m;
        bool _kupilLtcEth = true;//true = купил LTC за ETH

        decimal _eosBtc = 0;
        decimal _eosBtcRec = 0.000175m;
        bool _kupilEosBtc = true;//true = купил EOS за BTC


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

            _currencyPairs = new Dictionary<int, List<string>>();
            //List<string> itemPair = new List<string> { "LTC", "BTC"};
            //_currencyPairs.Add(1, itemPair);
            List<string> itemPair1 = new List<string> { _ltc, _eth };
            _currencyPairs.Add(2, itemPair1);
            List<string> itemPair2 = new List<string> { _ltc, _btc };
            _currencyPairs.Add(3, itemPair2);
            List<string> itemPair3 = new List<string> { _xrp, _btc };
            _currencyPairs.Add(4, itemPair3);
            List<string> itemPair4 = new List<string> { _eos, _btc };
            _currencyPairs.Add(5, itemPair4);

            _tradesInfoList = new List<Models.TradeInfo>();

            _tradesProccedList = new List<Models.TradeInfo>();
            Models.TradeInfo newItem = new Models.TradeInfo();
            newItem.Symbol = _xrp + "/" + _btc;
            newItem.PurchaseRate = _xrpBtc;
            newItem.SellingRate = 0;
            newItem.UpdatedDate = new DateTime(2020, 12, 9, 11, 0, 0);
            newItem.Comment = "Купил XRP. Надо продать не менее чем за " + _xrpBtcRec.ToString();

            _tradesProccedList.Add(newItem);

            Models.TradeInfo newItem1 = new Models.TradeInfo();
            newItem1.Symbol = _ltc + "/" + _eth;
            newItem1.PurchaseRate = _ltcEth;
            newItem1.SellingRate = 0;
            newItem1.UpdatedDate = DateTime.Now;
            newItem1.Comment = "Надо продать LTC не менее чем за " + _ltcEthRec.ToString();
            _tradesProccedList.Add(newItem1);

            Models.TradeInfo newItem2 = new Models.TradeInfo();
            newItem2.Symbol = _ltc + "/" + _btc;
            newItem2.PurchaseRate = _ltcBtc;
            newItem2.SellingRate = 0;
            newItem2.UpdatedDate = DateTime.Now;
            newItem2.Comment = "Надо продать LTC не менее чем за " + _ltcBtcRec.ToString();
            _tradesProccedList.Add(newItem2);

            Models.TradeInfo newItem3 = new Models.TradeInfo();
            newItem3.Symbol = _eos + "/" + _btc;
            newItem3.PurchaseRate = _eosBtc;
            newItem3.SellingRate = 0;
            newItem3.UpdatedDate = DateTime.Now;
            newItem3.Comment = "Можно продать EOS не менее чем за " + _eosBtcRec.ToString();
            _tradesProccedList.Add(newItem3);


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
                            if (pair.Symbol.Contains(_xrp))
                            {
                                pair.Color = "red";
                                if (_kupilXrp)
                                {
                                    if (pair.PurchaseRate > _xrpBtcRec) pair.Color = "green";
                                }
                                else if (pair.PurchaseRate < _xrpBtcRec) pair.Color = "green";
                            }
                            if (pair.Symbol.Contains(_ltc) && pair.Symbol.Contains(_btc))
                            {
                                pair.Color = "red";
                                if (_kupilLtc)
                                {
                                    if (pair.PurchaseRate > _ltcBtcRec) pair.Color = "green";
                                }
                                else if (pair.PurchaseRate < _ltcBtcRec) pair.Color = "green";
                            }
                            if (pair.Symbol.Contains(_ltc) && pair.Symbol.Contains(_eth))
                            {
                                pair.Color = "red";
                                if (_kupilLtcEth)
                                {
                                    if (pair.PurchaseRate > _ltcEthRec) pair.Color = "green";
                                }
                                else if (pair.PurchaseRate < _ltcEthRec) pair.Color = "green";
                            }
                            if (pair.Symbol.Contains(_eos) && pair.Symbol.Contains(_btc))
                            {
                                pair.Color = "red";
                                if (_kupilEosBtc)
                                {
                                    if (pair.PurchaseRate > _eosBtcRec) pair.Color = "green";
                                }
                                else if (pair.PurchaseRate < _eosBtcRec) pair.Color = "green";
                            }
                            if(pair.Color.Contains("green")) Alert = true;
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
    }
}