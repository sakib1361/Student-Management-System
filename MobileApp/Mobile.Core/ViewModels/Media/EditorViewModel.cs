using GalaSoft.MvvmLight.Command;
using Mobile.Core.ViewModels.Core;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class EditorViewModel : BaseViewModel, IPopupModel
    {
        public string Data { get; set; } = string.Empty;
        public ICommand DataCommand { get; set; }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args.Length > 0 && args[0] is string data)
            {
                Data = data;
            }
        }

        public ICommand CloseCommand => new RelayCommand(CloseAction);

        private void CloseAction()
        {
            DataCommand?.Execute(Data);
            _nav.GoModalBack();
        }
    }
}
