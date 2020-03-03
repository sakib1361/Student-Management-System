using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Notice
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoticeDetailPage : CustomPage<NoticeDetailViewModel>
    {
        public NoticeDetailPage()
        {
            InitializeComponent();
        }
    }
}