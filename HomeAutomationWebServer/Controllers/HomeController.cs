using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HomeAutomationWebServer.Helpers;
using HomeAutomationWebServer.Models;
using HomeAutomationWebServer.Models.Items;
using OpenHardwareMonitor.Hardware;

namespace HomeAutomationWebServer.Controllers
{
    public class HomeController : Controller
    {
        private HomeInfoModel _infoModel;
        private Computer _cpDevices;

        public HomeController()
        {
            _infoModel = new HomeInfoModel();
            _infoModel.CpuInfo = new CpuItems();
            _infoModel.CpuInfo.items = new List<DeviceModel>();
            _infoModel.VideoInfo = new List<DeviceModel>();

            _cpDevices = new Computer();
            _cpDevices.Open();
            _cpDevices.GPUEnabled = true;
            _cpDevices.MainboardEnabled = true;
            _cpDevices.CPUEnabled = true;
        }

        private void UpdateSensors()
        {
            for (int i = 0; i < _cpDevices.Hardware.Count(); i++)
            {
                if (_cpDevices.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    _infoModel.CpuInfo.Name = _cpDevices.Hardware[i].Name;
                    _infoModel.CpuInfo.Power = 0;
                    _infoModel.CpuInfo.Load = 0;
                    foreach (ISensor sensor in _cpDevices.Hardware[i].Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            DeviceModel cpuDev = new DeviceModel();
                            cpuDev.Name = sensor.Name;
                            cpuDev.Temperature = sensor.Value.HasValue ? sensor.Value.Value : 0;
                            _infoModel.CpuInfo.items.Add(cpuDev);
                        }
                        else if (sensor.SensorType == SensorType.Power)
                        {
                            _infoModel.CpuInfo.Power += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                        {
                            _infoModel.CpuInfo.Load += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                    }
                }
                if (_cpDevices.Hardware[i].HardwareType == HardwareType.GpuAti ||
                    _cpDevices.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    foreach (ISensor sensor in _cpDevices.Hardware[i].Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            DeviceModel vm = new DeviceModel();
                            vm.Name = _cpDevices.Hardware[i].Name;
                            vm.Temperature = sensor.Value.HasValue ? sensor.Value.Value : 0;
                            if (vm.Temperature > 100) vm.Temperature = 5.0 / 9.0 * (vm.Temperature - 32);
                            _infoModel.VideoInfo.Add(vm);
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
        }
        public ActionResult Index()
        {
            _infoModel.CurrentDateTime = DateTime.Now;
            _infoModel.CpuInfo.items.Clear();
            _infoModel.VideoInfo.Clear();
            UpdateSensors();
            return View(_infoModel);
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

        [HttpPost]
        [AllowAnonymous]
        public JsonResult UpdateCpuInfo()
        {
            UpdateSensors();
            var jsonSerialiser = new JavaScriptSerializer();
            var jsonPR = jsonSerialiser.Serialize(_infoModel.CpuInfo);

            return new JsonResult { Data = new { jsonPR, isSuccess = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}