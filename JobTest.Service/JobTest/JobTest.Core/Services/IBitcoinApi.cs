using System.Threading;
using System.Threading.Tasks;
using JobTest.Core.Data;
using Refit;

namespace JobTest.Core.Services
{
    public interface IBitcoinApi
    {
        [Get("/v1/bpi/currentprice.json")]
        Task<BitcoinDataResponse> GetBitcoinData(CancellationToken cancellationToken);
    }
}