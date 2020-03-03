using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilleDetailPage : CustomPage<ProfileDetailViewModel>
    {
        public ProfilleDetailPage()
        {
            InitializeComponent();
        }
    }
}