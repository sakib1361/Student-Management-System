using CoreEngine.Model.Common;
using Mobile.Core.Engines.Dependency;
using Mobile.Core.Engines.Services;
using Mobile.Core.ViewModels;
using Mobile.Core.ViewModels.Core;
using MobileApp.Views.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.Service
{
    public class NavigationService : INavigationService, IDialogService
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly Dictionary<Type, Type> Pages;
        private readonly IPlatformService _platformService;
        private Type CurrentPage;

        private NavigationPage _nav;

        public NavigationService(IPlatformService platformService)
        {
            Pages = new Dictionary<Type, Type>();
            _platformService = platformService;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public async void Init<T>() where T : BaseViewModel
        {
            await _semaphoreSlim.WaitAsync();
            var vm = typeof(T);
            if (CurrentPage == vm)
            {
                return;
            }
            CurrentPage = vm;
            var page = Activator.CreateInstance(Pages[vm]) as Page;
            page.BindingContext = Locator.GetInstance<T>();


            if (vm == typeof(HomeViewModel))
            {
                _nav = new NavigationPage(page)
                {
                    BarBackgroundColor = Color.DarkBlue,
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = new MainPage(_nav);
            }
            else
            {
                _nav = new NavigationPage(page)
                {
                    BarBackgroundColor = Color.DarkBlue,
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = _nav;
            }

            

            if (page.BindingContext is BaseViewModel viewModel)
            {
                viewModel.OnAppear();
            }

            _semaphoreSlim.Release();
            _nav.Popped += (s, e) =>
            {
                CurrentPage = _nav.CurrentPage.BindingContext.GetType();
            };
        }

        public async void GoBack()
        {
            await _semaphoreSlim.WaitAsync();
            await _nav.Navigation.PopAsync();
            _semaphoreSlim.Release();
        }

        public async void GoModalBack()
        {
            await _semaphoreSlim.WaitAsync();
            await _nav.Navigation.PopModalAsync();
            _semaphoreSlim.Release();
        }

        public async void GoToRoot()
        {
            await _semaphoreSlim.WaitAsync();
            await _nav.Navigation.PopToRootAsync();
            _semaphoreSlim.Release();
        }


        public void NavigateTo<T>(params object[] parameter) where T : BaseViewModel
        {
            var type = typeof(T);
            NavigateTo(type, parameter);
        }

        public async void NavigateTo(Type type, params object[] parameter)
        {
            if (type == CurrentPage)
            {
                return;
            }
            else if (type == typeof(HomeViewModel))
            {
                await _nav.PopToRootAsync();
                return;
            }
            CurrentPage = type;
            await _semaphoreSlim.WaitAsync();
            if (Pages.ContainsKey(type))
            {
                var page = Activator.CreateInstance(Pages[type]) as Page;
                page.BindingContext = Locator.GetInstance(type);
                await _nav.Navigation.PushAsync(page);
                if (page.BindingContext is BaseViewModel viewModel)
                {
                    viewModel.OnAppear(parameter);
                }

            }
            else
            {
                ShowMessage("Error", "Invalid Page");
            }
            _semaphoreSlim.Release();
        }

        public async Task NavigateToModal<T>(ICommand dataCommand, params object[] parameter) where T : BaseViewModel
        {
            await _semaphoreSlim.WaitAsync();
            var type = typeof(T);
            if (Pages.ContainsKey(type))
            {
                var page = Activator.CreateInstance(Pages[type]) as Page;
                // var vm = Locator.GetInstance<T>();
                // page.BindingContext = vm;
                await _nav.Navigation.PushModalAsync(page);
                if (page.BindingContext is BaseViewModel viewModel)
                {
                    viewModel.OnAppear(parameter);
                }

                if (page.BindingContext is IPopupModel popupModel)
                {
                    popupModel.DataCommand = dataCommand;
                }
            }
            else
            {
                ShowMessage("Error", "Invalid Page");
            }
            _semaphoreSlim.Release();
        }

        public void Configure(Type baseViewModel, Type page)
        {
            Pages.Add(baseViewModel, page);
        }

        public void ShowMessage(string title, string message)
        {
            _nav.DisplayAlert(title, message, "Ok");
        }

        public async Task<bool> ShowConfirmation(string title, string message)
        {
            var res = await _nav.DisplayAlert(title, message, "Ok", "Cancel");
            return res;
        }

        public async Task<string> ShowAction(string title, string cancel, params string[] actions)
        {
            var res = await _nav.DisplayActionSheet(title, cancel, "", actions);
            return res;
        }

        public async void ShowAction(string title, string cancel, Dictionary<string, Action> actions)
        {
            var buttons = actions.Keys.ToArray();
            var res = await ShowAction(title, cancel, buttons);
            if (!string.IsNullOrWhiteSpace(res) && actions.ContainsKey(res))
            {
                actions[res].Invoke();
            }
        }

        public void ShowToastMessage(string message)
        {
            _platformService.OpenToast(message);
        }

        public void OpenFile(string fileId)
        {
            var url = Path.Combine(AppConstants.BaseUrl + "api/files/index?id=" + fileId);
            Launcher.TryOpenAsync(url);
        }
    }
}
