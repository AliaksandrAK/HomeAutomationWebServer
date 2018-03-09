// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using HomeAutomationWebServer.Helpers;
//   https://app.quicktype.io/#l=cs&r=json2csharp
//    var welcome = Welcome.FromJson(jsonString);

namespace HomeAutomationWebServer.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Welcome
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("Children")]
        public List<Welcome> Children { get; set; }

        [JsonProperty("Min")]
        public string Min { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Max")]
        public string Max { get; set; }

        [JsonProperty("ImageURL")]
        public string ImageUrl { get; set; }
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Welcome>(json, HomeAutomationWebServer.Helpers.Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self)
        {
            return JsonConvert.SerializeObject(self, HomeAutomationWebServer.Helpers.Converter.Settings);
        }
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
