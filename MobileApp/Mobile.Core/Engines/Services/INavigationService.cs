using Mobile.Core.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.Core.Engines.Services
{
    public interface INavigationService
    {
        void GoBack();
        void GoToRoot();
        void NavigateTo<T>(params object[] parameter) where T : BaseViewModel;
        void NavigateTo(Type T, params object[] parameter);
        Task NavigateToModal<T>(ICommand dataCommand, params object[] parameter) where T : BaseViewModel;
        void GoModalBack();
        void Configure(Type viewModel, Type page);
        void Init<T>() where T : BaseViewModel;
        void OpenFile(string fileId);
    }
}
