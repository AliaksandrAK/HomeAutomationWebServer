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
            HomeInfoModel infoModel = new HomeInfoModel();
            infoModel.CurrentDateTime = DateTime.Now;
            infoModel.CpuInfo = new CpuItems();
            infoModel.CpuInfo.items = new List<DeviceModel>();
            infoModel.VideoInfo = new List<DeviceModel>();

            var cp = new Computer();
            cp.Open();
            cp.GPUEnabled = true;
            cp.MainboardEnabled = true;
            cp.CPUEnabled = true;
            for (int i = 0; i < cp.Hardware.Count(); i++)
            {
                if (cp.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    infoModel.CpuInfo.Name = cp.Hardware[i].Name;
                    infoModel.CpuInfo.Power = 0;
                    infoModel.CpuInfo.Load = 0;
                    foreach (ISensor sensor in cp.Hardware[i].Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            DeviceModel cpuDev = new DeviceModel();
                            cpuDev.Name = sensor.Name;
                            cpuDev.Temperature = sensor.Value.HasValue ? sensor.Value.Value : 0;
                            infoModel.CpuInfo.items.Add(cpuDev);
                        }
                        else if (sensor.SensorType == SensorType.Power)
                        {
                            infoModel.CpuInfo.Power += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                        {
                            infoModel.CpuInfo.Load += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                    }
                }
                if (cp.Hardware[i].HardwareType == HardwareType.GpuAti ||
                    cp.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    foreach (ISensor sensor in cp.Hardware[i].Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            DeviceModel vm = new DeviceModel();
                            vm.Name = cp.Hardware[i].Name;
                            vm.Temperature = sensor.Value.HasValue ? sensor.Value.Value : 0;
                            if(vm.Temperature>100) vm.Temperature = 5.0 / 9.0 * (vm.Temperature - 32);
                            infoModel.VideoInfo.Add(vm);
                        }
                    }
                }
            }

            /*
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
            */
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