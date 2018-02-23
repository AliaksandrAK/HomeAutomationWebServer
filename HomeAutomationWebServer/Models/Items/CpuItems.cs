using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAutomationWebServer.Models.Items
{
    public class CpuItems
    {
        public string Name { get; set; }
        public double Power { get; set; }
        public double Load { get; set; }
        public List<DeviceModel> items { get; set; }
    }
}