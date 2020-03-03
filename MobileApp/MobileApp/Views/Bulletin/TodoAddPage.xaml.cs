using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Bulletin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoAddPage : CustomPage<TodoAddViewModel>
    {
        public TodoAddPage()
        {
            InitializeComponent();
        }
    }
}