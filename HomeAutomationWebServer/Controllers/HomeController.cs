using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeAutomationWebServer.Helpers;
using HomeAutomationWebServer.Models;
using HomeAutomationWebServer.Models.Items;
using OpenHardwareMonitor.Hardware;

namespace HomeAutomationWebServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cp = new Computer();
            cp.Open();
            cp.GPUEnabled = true;
            cp.MainboardEnabled = true;
            for(int i = 0; i<cp.Hardware.Count(); i++)
            {
                if (cp.Hardware[i].HardwareType == HardwareType.Mainboard)
                {
                    
                }
            }




            HomeInfoModel infoModel = new HomeInfoModel();
            infoModel.CurrentDateTime = DateTime.Now;
            infoModel.CpuInfo = new DeviceModel();
            infoModel.CpuInfo.Name = HardwareInfo.GetProcessorInformation();
            infoModel.CpuInfo.Temperature = HardwareInfo.GetTemperature();
            infoModel.VideoInfo = new List<DeviceModel>();
            var videoList = HardwareInfo.GetVideoCardsName();
            foreach (var video in videoList)
            {
                DeviceModel vm = new DeviceModel();
                vm.Name = video;
                vm.Temperature = 1;
                infoModel.VideoInfo.Add(vm);
            }
            return View(infoModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}