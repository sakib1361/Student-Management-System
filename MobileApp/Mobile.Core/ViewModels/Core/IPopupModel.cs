using System.Windows.Input;

namespace Mobile.Core.ViewModels.Core
{
    public interface IPopupModel
    {
        ICommand DataCommand { get; set; }
    }
}
