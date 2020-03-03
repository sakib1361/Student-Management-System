
using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Splash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : CustomPage<SplashViewModel>
    {
        public SplashPage()
        {
            InitializeComponent();
        }
    }
}