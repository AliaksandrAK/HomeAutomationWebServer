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
            var myPairs = cryptoServ.GetMyPairsInfo();

            ViewBag.Trades = tradeInfo;
            ViewBag.Pairs = myPairs;
            ViewBag.UpdatedDate = tradeInfo.Count > 0 ? tradeInfo[0].UpdatedDate : DateTime.Now;

            return View();
        }
    }
}