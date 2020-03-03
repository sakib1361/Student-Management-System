using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : CustomPage<StudentsViewModel>
    {
        public StudentsPage()
        {
            InitializeComponent();
        }
    }
}