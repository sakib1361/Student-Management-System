using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Course
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesPage : CustomPage<CoursesViewModel>
    {
        public CoursesPage()
        {
            InitializeComponent();
        }
    }
}