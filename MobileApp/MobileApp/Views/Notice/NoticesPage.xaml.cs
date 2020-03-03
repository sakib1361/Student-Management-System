using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Notice
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoticesPage : CustomTabPage<NoticesViewModel>
    {
        public NoticesPage()
        {
            InitializeComponent();
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            AllNoticeController.IsRefreshing = true;
        }
    }
}