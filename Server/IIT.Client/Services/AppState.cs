using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System;
using System.Threading.Tasks;

namespace IIT.Client.Services
{
    public class AppState
    {
        private string _currentToken;
        private readonly IMemberHandler _memberHandler;
        private readonly HttpWorker _httpWorker;
        private readonly ILocalStorageService _localData;
        private readonly ISessionStorageService _sessionData;
        private DBUser CurrentUser;
        private const string tokenKey = "__access_token__";
        private const string tokenUser = "__access_user__";

        public AppState(IMemberHandler memberHandler, HttpWorker httpWorker,
            ILocalStorageService localStorageService,
            ISessionStorageService sessionStorageService)
        {
            _memberHandler = memberHandler;
            _httpWorker = httpWorker;
            _localData = localStorageService;
            _sessionData = sessionStorageService;
        }


        internal async Task<DBUser> GetUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentToken))
                {
                    if (await _localData.ContainKeyAsync(tokenKey))
                    {
                        _currentToken = await _localData.GetItemAsync<string>(tokenKey);
                    }
                }
            }
            catch { }
            if (!string.IsNullOrWhiteSpace(_currentToken))
            {
                _httpWorker.LoggedIn(_currentToken);
                if (CurrentUser != null)
                {
                    return CurrentUser;
                }

                try
                {
                    var user = await _sessionData.GetItemAsync<DBUser>(tokenUser);
                    if (user != null)
                    {
                        CurrentUser = user;
                        return user;
                    }
                }
                catch { }

                var res = await _memberHandler.TouchLogin();
                if (res == null)
                {
                    await Logout();
                    return null;
                }
                else
                {
                    CurrentUser = res;
                    await _sessionData.SetItemAsync(tokenUser, CurrentUser);
                    return res;
                }
            }




            Console.WriteLine("Asking For Info: " + _currentToken);


            return null;
        }

        internal async Task Logout()
        {
            _memberHandler.Logout();
            _currentToken = string.Empty;
            CurrentUser = null;
            await _localData.ClearAsync();
            await _sessionData.ClearAsync();
        }

        internal async void Login(SignInResponse res, bool remember)
        {
            if (res.Success)
            {
                _currentToken = res.Token;
                await _localData.SetItemAsync(tokenKey, _currentToken);
            }
        }
    }
}
