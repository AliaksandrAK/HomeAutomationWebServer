﻿using System;
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
            _cpDevices.GPUEnabled = true;
            _cpDevices.MainboardEnabled = true;
            _cpDevices.CPUEnabled = true;
            _cpDevices.Open();
        }

        private void UpdateSensors()
        {
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
        public JsonResult UpdateCompInfo()
        {
            UpdateSensors();
            var jsonSerialiser = new JavaScriptSerializer();
            var jsonPR = jsonSerialiser.Serialize(_infoModel);

            return new JsonResult { Data = new { jsonPR, isSuccess = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}