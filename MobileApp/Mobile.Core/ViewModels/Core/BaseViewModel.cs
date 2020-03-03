using CoreEngine.Model.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using Mobile.Core.Models.Core;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    //[AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel : ViewModelBase
    {
        public bool IsBusy { get; set; }
        public bool IsRefreshisng { get; set; }
        protected readonly INavigationService _nav;
        protected readonly IDialogService _dialog;
        public BaseViewModel()
        {
            _nav = AppService.Nav;
            _dialog = AppService.Dialog;
        }
        public ICommand RefreshCommand => new RelayCommand(RefreshAction);

        public virtual void OnAppear(params object[] args)
        {

        }

        protected virtual void RefreshAction()
        {

        }

        protected void GoBack()
        {
            _nav.GoBack();
        }

        protected void ShowResponse(ActionResponse response, bool goBackOnSUccess = false)
        {
            if (response != null)
            {
                _dialog.ShowToastMessage(response.Message);
                if (goBackOnSUccess) GoBack();
            }
        }
    }
}
