using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HomeAutomationWebServer.Controllers
{
    public class CameraController : Controller
    {
        // GET: Camera
        public ActionResult Index()
        {
            return View();
            //return Redirect("http://10.0.43.54:8000/");
        }
        // GET: IpCameras
        public ActionResult IpCameras()
        {
            //check if there is Ip camera then 
            return View();
            //if there is not IP camera then go to Info page
        }
        public ActionResult LocalCameras()
        {
            //different approach in html  - window.location.replace('http://localhost:8000/

            var ipAd = NetworkInterface.GetAllNetworkInterfaces()
                           .SelectMany(adapter => adapter.GetIPProperties().UnicastAddresses)
                           .Where(adr => adr.Address.AddressFamily == AddressFamily.InterNetwork && adr.IsDnsEligible)
                           .Select(adr => adr.Address.ToString()).FirstOrDefault();

            //To get the local IP address 
            string sHostName = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostByName(sHostName);
            IPAddress[] IpA = ipE.AddressList;

            string sysPath = "http://";
            if (IpA.Length > 0) sysPath = "http://" + IpA[0].ToString() + ":8000/";
            else sysPath = "http://localhost:8000/";
            return Redirect(sysPath);
        }
    }
}