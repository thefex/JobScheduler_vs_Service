using System;
using System.Reflection;
using JobTest.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Refit.Insane.PowerPack.Configuration;
using Refit.Insane.PowerPack.Services;

namespace JobTest.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            BaseApiConfiguration.ApiUri = new Uri("https://api.coindesk.com");
            BaseApiConfiguration.Timeout = TimeSpan.FromSeconds(15);
            
            Mvx.RegisterType<IRestService>(() => 
                new RestServiceBuilder()
                    .BuildRestService(typeof(IBitcoinApi).GetTypeInfo().Assembly)
                    );
            
            Mvx.RegisterType<BitcoinPriceService>(
                () => new BitcoinPriceService(Mvx.Resolve<IRestService>()));

            RegisterNavigationServiceAppStart<ViewModels.MainViewModel>();
        }
    }
}
