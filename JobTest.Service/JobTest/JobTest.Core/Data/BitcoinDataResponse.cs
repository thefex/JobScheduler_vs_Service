using Newtonsoft.Json;

namespace JobTest.Core.Data
{
    public class BitcoinDataResponse
    {
        [JsonProperty("time")]
        public Time Time { get; set; }

        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty("chartName")]
        public string ChartName { get; set; }

        [JsonProperty("bpi")]
        public Price Price { get; set; }
    }
}