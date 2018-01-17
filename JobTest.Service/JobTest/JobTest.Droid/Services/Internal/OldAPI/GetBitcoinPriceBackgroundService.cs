using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using JobTest.Core.Services;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using Refit;

namespace JobTest.Droid.Services.Internal.OldAPI
{
    [Service]
    public class GetBitcoinPriceBackgroundService : IntentService
    {
        public GetBitcoinPriceBackgroundService()
        {
        }

        public GetBitcoinPriceBackgroundService(IntPtr ptr, JniHandleOwnership transfer) : base(ptr, transfer)
        {

        }

        protected override async void OnHandleIntent(Intent intent)
        {
            // mvvmcross might not be initialized when service starts
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(this.ApplicationContext);
            setupSingleton.EnsureInitialized();

            await FetchAndDisplayBitcoinPrice(intent);

            return;
        }

        private async Task FetchAndDisplayBitcoinPrice(Intent intent)
        {
            var bitcoinPriceService = Mvx.Resolve<BitcoinPriceService>();

            try
            {
                var bitcoinDataResponse = await bitcoinPriceService.QueryForBitcoinData();

                // show toast on main thread
                using (Handler handler = new Handler(Looper.MainLooper))
                {
                    handler.Post(() =>
                    {
                        if (bitcoinDataResponse.IsSuccess)
                            Toast.MakeText(ApplicationContext, "Bitcoin price for date: " + bitcoinDataResponse.Results.Time.Updated + " is " + bitcoinDataResponse.Results.Price.Usd.RateFloat + " USD.", ToastLength.Long)
                                 .Show();
                        else
                            Toast.MakeText(ApplicationContext, bitcoinDataResponse.FormattedErrorMessages, ToastLength.Long)
                                 .Show();
                    });
                }
            }
            catch (ApiException e)
            {
                using (Handler handler = new Handler(Looper.MainLooper))
                {
                    handler.Post(() =>
                    {
                        Toast.MakeText(ApplicationContext, "HTTP call ended without success - response code: " + e.StatusCode, ToastLength.Long)
                             .Show();
                    });
                }
            }
            finally
            {
                // we have used WAKEFULBroadcastReceiver - which set wake lock
                // that means that device CPU should not go "sleep" till we release wake lock
                // (at least in theory, not valid in new apis anymore and some manufactuers as wakelocks are being ignored:) )
                PeriodicAlarmReceiver.CompleteWakefulIntent(intent);
            }
        }
    }
}
