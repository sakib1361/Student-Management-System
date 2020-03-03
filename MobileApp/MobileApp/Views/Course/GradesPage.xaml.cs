
using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Course
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GradesPage : CustomPage<GradesViewModel>
    {
        public GradesPage()
        {
            InitializeComponent();
        }
    }
}