using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAutomationWebServer.Models.Items;

namespace HomeAutomationWebServer.Models
{
    public class HomeInfoModel
    {
        public DateTime CurrentDateTime { get; set; }
        public CpuItems CpuInfo { get; set; }
        public List<DeviceModel> VideoInfo { get; set; }
    }
}