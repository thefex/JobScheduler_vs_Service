using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using JobTest.Core.Services;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using Refit;

namespace JobTest.Droid.Services.Internal.NewAPI
{
    [Service(Permission = "android.permission.BIND_JOB_SERVICE")]
    public class GetBitcoinPriceJob : JobService
    {
        public GetBitcoinPriceJob()
        {
        }

        public GetBitcoinPriceJob(IntPtr ptr, JniHandleOwnership transfer)
        {

        }

        public override bool OnStartJob(JobParameters @params)
        {
            // mvvmcross might not be initialized here
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(this.ApplicationContext);
            setupSingleton.EnsureInitialized();

            Task.Run(async () =>
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
                    JobFinished(@params, true);
                }
            });


            return true; // return true if job deferred to other thread (task longer than 16ms)
        }

        public override bool OnStopJob(JobParameters @params)
        {
            // this is the place where resources should be released
            // it is called when job finished prematurely

            // return true if job has to be rescheduled
            return true;
        }
    }
}
