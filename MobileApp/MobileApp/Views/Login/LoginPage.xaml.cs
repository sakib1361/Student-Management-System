using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : CustomPage<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
        }
    }
}