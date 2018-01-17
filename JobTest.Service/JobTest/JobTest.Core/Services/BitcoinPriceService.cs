using System.Threading;
using System.Threading.Tasks;
using JobTest.Core.Data;
using Refit.Insane.PowerPack.Data;
using Refit.Insane.PowerPack.Services;

namespace JobTest.Core.Services
{
    public class BitcoinPriceService
    {
        private readonly IRestService _restService;

        public BitcoinPriceService(IRestService restService)
        {
            _restService = restService;
        }
        
        public Task<Response<BitcoinDataResponse>> QueryForBitcoinData()
        {
            return _restService.Execute<IBitcoinApi, BitcoinDataResponse>(api => api.GetBitcoinData(default(CancellationToken)));
        }
    }
}