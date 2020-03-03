using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Client.Services
{
    public class ApiAuthenticationProvider : AuthenticationStateProvider
    {
        private readonly IMemberHandler _memberHandler;
        private readonly AppState _appState;


        public ApiAuthenticationProvider(IMemberHandler memberHandler, AppState appState)
        {
            _memberHandler = memberHandler;
            _appState = appState;
        }

        public async Task<SignInResponse> Login(string username, string password, bool remember)
        {
            var res = await _memberHandler.Login(username, password);
            if (res != null && res.Success)
            {
                _appState.Login(res, remember);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }

            return res;
        }

        public async Task Logout()
        {
            await _appState.Logout();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _appState.GetUser();
            if (currentUser == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                var identity = new ClaimsIdentity(new[]
                 {
                    new Claim(ClaimTypes.Name, currentUser.UserName),
                    new Claim(ClaimTypes.Role, currentUser.Role)
                }, "jwt");

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
        }
    }
}
