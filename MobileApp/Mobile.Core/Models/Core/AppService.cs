using CoreEngine.Model.DBModel;
using Mobile.Core.Engines.Services;

namespace Mobile.Core.Models.Core
{
    public class AppService
    {
        public static bool HasCRRole;
        public static INavigationService Nav { get; private set; }
        public static IDialogService Dialog { get; private set; }
        public static DBUser CurrentUser { get; internal set; }

        public static void Init(INavigationService navigationService, IDialogService dialogService)
        {
            Nav = navigationService;
            Dialog = dialogService;
        }

        public static void ShowAlert(string title, string body)
        {
            Dialog.ShowMessage(title, body);
        }
    }
}
