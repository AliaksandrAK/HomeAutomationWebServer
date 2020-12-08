using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HomeAutomationWebServer.Helpers;
using HomeAutomationWebServer.Models;
using HomeAutomationWebServer.Models.Items;
using OpenHardwareMonitor.Hardware;
using System.IO;
using System.Text;
using Newtonsoft.Json;

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
        }

        private void UpdateSensorsUsingMonitorLib()
        {
            _cpDevices = new Computer();
            _cpDevices.GPUEnabled = true;
            _cpDevices.MainboardEnabled = true;
            _cpDevices.CPUEnabled = true;
            _cpDevices.Open();

            _infoModel.CurrentDateTime = DateTime.Now;
            _infoModel.CpuInfo.items.Clear();
            _infoModel.VideoInfo.Clear();
            for (int i = 0; i < _cpDevices.Hardware.Count(); i++)
            {
                if (_cpDevices.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    _infoModel.CpuInfo.Name = _cpDevices.Hardware[i].Name;
                    _infoModel.CpuInfo.Power = 0;
                    _infoModel.CpuInfo.Load = 0;
                    int id = 0;
                    foreach (ISensor sensor in _cpDevices.Hardware[i].Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if(sensor.Value.HasValue)
                            {
                                DeviceModel cpuDev = new DeviceModel();
                                cpuDev.Id = id;
                                cpuDev.Name = sensor.Name;
                                cpuDev.Temperature = sensor.Value.Value;
                                _infoModel.CpuInfo.items.Add(cpuDev);
                            }
                        }
                        else if (sensor.SensorType == SensorType.Power)
                        {
                            _infoModel.CpuInfo.Power += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                        {
                            _infoModel.CpuInfo.Load += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                        id++;
                    }
                }
                if (_cpDevices.Hardware[i].HardwareType == HardwareType.GpuAti ||
                    _cpDevices.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    DeviceModel vm = new DeviceModel();
                    vm.Name = _cpDevices.Hardware[i].Name;
                    vm.Power = 0;
                    foreach (ISensor sensor in _cpDevices.Hardware[i].Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if(sensor.Value.HasValue)
                            {
                                vm.Temperature = sensor.Value.Value;
                                if (vm.Temperature > 100) vm.Temperature = 5.0 / 9.0 * (vm.Temperature - 32);
                            }
                        }
                        else if (sensor.SensorType == SensorType.Power)
                        {
                            vm.Power += sensor.Value.HasValue ? sensor.Value.Value : 0;
                        }
                    }
                    _infoModel.VideoInfo.Add(vm);
                }
            }
        }

        private void UpdateSystemInfo()
        {
            _infoModel.CurrentDateTime = DateTime.Now;
            _infoModel.CpuInfo.items.Clear();
            _infoModel.VideoInfo.Clear();

            _infoModel.CpuInfo.Name = HardwareInfo.GetProcessorInformation();
            DeviceModel cpuDev = new DeviceModel();
            cpuDev.Id = 0;
            cpuDev.Name = "Core";
            cpuDev.Temperature = HardwareInfo.GetTemperature();
            var vpuSpeed = HardwareInfo.GetCpuSpeedInGHz();
            cpuDev.Power = vpuSpeed.HasValue ? vpuSpeed.Value : 0;
            _infoModel.CpuInfo.items.Add(cpuDev);

            _infoModel.VideoInfo = new List<DeviceModel>();
            var videoList = HardwareInfo.GetVideoCardsName();
            foreach (var video in videoList)
            {
                DeviceModel vm = new DeviceModel();
                vm.Name = video;
                vm.Temperature = 1;
                _infoModel.VideoInfo.Add(vm);
            }
        }
        private void UpdateSystemInfoFromFile()
        {
            _infoModel.CurrentDateTime = DateTime.Now;
            _infoModel.CpuInfo.items.Clear();
            _infoModel.VideoInfo.Clear();
            string path = Server.MapPath("~/bin/system_info.json");
            FileStream sourceFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (sourceFile.Length > 0)
            {
                byte[] getContent = new byte[(int)sourceFile.Length];
                UTF8Encoding temp = new UTF8Encoding(true);
                sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                String json = temp.GetString(getContent);
                //Welcome sysInfo = Welcome.FromJson(json);
                _infoModel = JsonConvert.DeserializeObject<HomeInfoModel>(json);

                sourceFile.Close();
            }
        }
        public ActionResult Index()
        {
            UpdateSystemInfoFromFile();
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

        public ActionResult System()
        {
            var ipAd = NetworkInterface.GetAllNetworkInterfaces()
                        .SelectMany(adapter => adapter.GetIPProperties().UnicastAddresses)
                        .Where(adr => adr.Address.AddressFamily == AddressFamily.InterNetwork && adr.IsDnsEligible)
                        .Select(adr => adr.Address.ToString()).FirstOrDefault();

            //To get the local IP address 
            string sHostName = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostEntry(sHostName);
            IPAddress[] IpA = ipE.AddressList;

            string sysPath = "http://";
            if (IpA.Length > 0) sysPath = "http://" + IpA[3].ToString() + ":8085/";
            else sysPath = "http://localhost:8085/";
            return Redirect(sysPath);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult UpdateCompInfo()
        {
            UpdateSystemInfoFromFile();
            var jsonSerialiser = new JavaScriptSerializer();
            var jsonPR = jsonSerialiser.Serialize(_infoModel);

            return new JsonResult { Data = new { jsonPR, isSuccess = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}