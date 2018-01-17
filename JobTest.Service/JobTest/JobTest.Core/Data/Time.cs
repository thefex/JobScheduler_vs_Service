using System;
using Newtonsoft.Json;

namespace JobTest.Core.Data
{
    public class Time
    {
        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("updatedISO")]
        public DateTime UpdatedIso { get; set; }

        [JsonProperty("updateduk")]
        public string Updateduk { get; set; }
    }
}