using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}