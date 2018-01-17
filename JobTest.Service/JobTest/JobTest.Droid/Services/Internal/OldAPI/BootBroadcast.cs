using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;

namespace JobTest.Droid.Services.Internal.OldAPI
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootBroadcast : BroadcastReceiver
    {
        public BootBroadcast()
        {

        }

        public BootBroadcast(IntPtr ptr, JniHandleOwnership transfer)
        {

        }

        // this will be called when device 
        public override void OnReceive(Context context, Intent intent)
        {
            // mvvmcross won't be initialized at this point
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
            setupSingleton.EnsureInitialized();

            var bitcoinPriceBackgroundServiceScheduler = Mvx.Resolve<BitcoinPriceBackgroundServiceScheduler>();

            // there should be check
            // if ( User Has Clicked Start Background Service Button in PAST
            // using some kind of storage 
            bitcoinPriceBackgroundServiceScheduler.StartPeriodicBackgroundWorkUsingAlarmManager();
        }
    }
}
