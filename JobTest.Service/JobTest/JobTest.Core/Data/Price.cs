using Newtonsoft.Json;

namespace JobTest.Core.Data
{
    public class Price
    {
        [JsonProperty("USD")]
        public Currency Usd { get; set; }

        [JsonProperty("GBP")]
        public Currency Gbp { get; set; }

        [JsonProperty("EUR")]
        public Currency Currency { get; set; }
    }
}