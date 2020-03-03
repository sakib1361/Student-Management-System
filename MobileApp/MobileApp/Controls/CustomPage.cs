using Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Controls
{
    public partial class CustomPage<T> : ContentPage where T : BaseViewModel
    {
    }

    public partial class CustomTabPage<T> : TabbedPage where T : BaseViewModel
    {

    }
}
