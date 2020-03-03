using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : CustomPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }
}