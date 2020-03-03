
using Mobile.Core.Engines.Dependency;
using Mobile.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage(Page page)
        {
            InitializeComponent();
            BindingContext = Locator.GetInstance<MainViewModel>();
            Detail = page;
        }
    }
}