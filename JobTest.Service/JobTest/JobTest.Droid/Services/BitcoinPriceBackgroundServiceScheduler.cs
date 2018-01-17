using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Widget;
using JobTest.Droid.Services.Internal.NewAPI;
using JobTest.Droid.Services.Internal.OldAPI;

namespace JobTest.Droid.Services
{
    public class BitcoinPriceBackgroundServiceScheduler
    {
        public BitcoinPriceBackgroundServiceScheduler()
        {
        }

        public void StartPeriodicBackgroundWorkUsingAlarmManager(){
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, new Intent(Application.Context, typeof(PeriodicAlarmReceiver)), PendingIntentFlags.UpdateCurrent);

            AlarmManager alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
            alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 1000, 10 * 1000, pendingIntent);
        }

        public void StartPeriodicBackgroundWorkUsingJobScheduler(){
            Java.Lang.Class javaClass = Java.Lang.Class.FromType(typeof(GetBitcoinPriceJob));
            ComponentName component = new ComponentName(Application.Context, javaClass);

            int jobId = 2300;
            JobInfo.Builder builder = new JobInfo.Builder(jobId, component)
                                                .SetPeriodic(1000*10) // start after every 10 seconds
                                                .SetPersisted(true) // job will work after device reboot
                                                .SetRequiredNetworkType(NetworkType.Any); // and require internet connection of any type
            JobInfo jobInfo = builder.Build();

            JobScheduler jobScheduler = (JobScheduler)Application.Context.GetSystemService(Context.JobSchedulerService);
            int result = jobScheduler.Schedule(jobInfo);

            using (var handler = new Handler(Looper.MainLooper))
            {
                if (result == JobScheduler.ResultSuccess)
                {
                    Toast.MakeText(Application.Context, "job succesfully scheduled", ToastLength.Short);
                    // The job was scheduled.
                }
                else
                {
                    Toast.MakeText(Application.Context, "job failed to schedule", ToastLength.Short);
                }
            }
        }
    }
}
