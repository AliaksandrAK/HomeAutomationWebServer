using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAutomationWebServer.Models.Items
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Temperature { get; set; }
    }
}