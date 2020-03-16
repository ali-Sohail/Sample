using Sample.Services;
using Xamarin.Forms;

namespace Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

            Xamarin.Forms.Device.SetFlags(new System.Collections.Generic.List<string>()
            {
            "StateTriggers_Experimental",
            "IndicatorView_Experimental",
            "CarouselView_Experimental",
            "MediaElement_Experimental"
            });
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}