using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Bulletin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoItemsPage : CustomPage<TodoItemsViewModel>
    {
        public TodoItemsPage()
        {
            InitializeComponent();
        }
    }
}