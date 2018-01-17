using Android.App;
using Android.OS;
using JobTest.Droid.Services;

namespace JobTest.Droid.Views
{
    [Activity()]
    public class MainView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.FirstView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);

            var bitcoinPriceBackgroundServiceScheduler = 
                new BitcoinPriceBackgroundServiceScheduler();

            FindViewById(Resource.Id.serviceButton).Click += (e,a)=>{
                bitcoinPriceBackgroundServiceScheduler.StartPeriodicBackgroundWorkUsingAlarmManager();
            };

            FindViewById(Resource.Id.jobSchedulerButton).Click += (e,a) => {
                bitcoinPriceBackgroundServiceScheduler.StartPeriodicBackgroundWorkUsingJobScheduler();
            };
        }
    }
}
