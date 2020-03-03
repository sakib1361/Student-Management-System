using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using System.Windows.Input;

namespace Mobile.Core.ViewModels.Login
{
    public class FacebookConnectViewModel : BaseViewModel
    {
        private readonly IFacebookService _facebookService;

        public FacebookConnectViewModel(IFacebookService facebookService)
        {
            _facebookService = facebookService;
        }
        public ICommand FacebookCommand => new RelayCommand(FacebookAction);
        public ICommand SkipCommand => new RelayCommand(SkipAction);

        private void SkipAction()
        {
            _nav.NavigateTo<HomeViewModel>();
        }

        private async void FacebookAction()
        {
            var res = await _facebookService.Login();
            if (res.Actionstatus)
            {
                _nav.NavigateTo<HomeViewModel>();
            }
            else
            {
                _dialog.ShowMessage("Error", res.Message);
            }
        }
    }
}
