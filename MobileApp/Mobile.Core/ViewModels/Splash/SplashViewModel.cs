using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using Mobile.Core.Engines.Services;
using Mobile.Core.Models.Core;
using Mobile.Core.Worker;

namespace Mobile.Core.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly HttpWorker _httpFactory;
        private readonly SettingService _settingServicce;
        private readonly IMemberHandler _memberHandler;
        private readonly IPlatformService _platformService;

        public SplashViewModel(HttpWorker httpWorker, IMemberHandler memberHandler, IPlatformService platformService, SettingService settingService)
        {
            _httpFactory = httpWorker;
            _settingServicce = settingService;
            _memberHandler = memberHandler;
            _platformService = platformService;
        }
        public override async void OnAppear(params object[] args)
        {
            if (string.IsNullOrWhiteSpace(_settingServicce.Token))
            {
                _nav.Init<LoginViewModel>();
            }
            else
            {
                _httpFactory.LoggedIn(_settingServicce.Token);
                var res = await _memberHandler.TouchLogin();
                if (res == null)
                {
                    _settingServicce.Token = string.Empty;
                    _httpFactory.Logout();
                    _nav.NavigateTo<LoginViewModel>();
                }
                else
                {
                    AppService.CurrentUser = res;
                    AppService.HasCRRole = res.ClassRepresentative;
                    _nav.Init<HomeViewModel>();
                    if (AppService.CurrentUser.Batch != null)
                    {
                        _platformService.SubsubcribeTopics(AppService.CurrentUser.Batch.Name);
                    }
                }
            }
        }
    }
}
