using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Media
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : CustomPage<EditorViewModel>
    {
        public EditorPage()
        {
            InitializeComponent();
        }
    }
}