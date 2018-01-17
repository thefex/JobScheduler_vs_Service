using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Content;

namespace JobTest.Droid.Services.Internal.OldAPI
{
    [BroadcastReceiver]
    public class PeriodicAlarmReceiver : WakefulBroadcastReceiver
    {
        public PeriodicAlarmReceiver()
        {

        }

        public PeriodicAlarmReceiver(IntPtr ptr, JniHandleOwnership transfer) : base(ptr, transfer)
        {

        }

        public override void OnReceive(Context context, Intent intent)
        {
            var backgroundServiceIntent = new Intent(context, typeof(GetBitcoinPriceBackgroundService));
            StartWakefulService(context, backgroundServiceIntent);
        }
    }
}
