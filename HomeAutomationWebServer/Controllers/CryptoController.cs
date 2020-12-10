using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoCurrency;

namespace HomeAutomationWebServer.Controllers
{
    public class CryptoController : Controller
    {
        // GET: Crypto
        public ActionResult Index()
        {
            CryptoInfoService cryptoServ = new CryptoInfoService();
            var tradeInfo = cryptoServ.GetRecommendationInfo();
            bool alert;
            var myPairs = cryptoServ.GetMyPairsInfo(out alert);
            var myPairsProc = cryptoServ.GetPairsProcessed();

            ViewBag.Trades = tradeInfo;
            ViewBag.Pairs = myPairs;
            ViewBag.PairsProc = myPairsProc;
            ViewBag.Alert = alert;
            ViewBag.UpdatedDate = tradeInfo.Count > 0 ? tradeInfo[0].UpdatedDate : DateTime.Now;

            return View();
        }
    }
}