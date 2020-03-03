using CoreEngine.Model.Common;
using Mobile.Core.Engines.Services;
using System.Threading.Tasks;

namespace MobileApp.Service
{
    public class FacebookService : IFacebookService
    {
        public bool IsLoggedIn => CheckLogin();

        private bool CheckLogin()
        {
            return false;
        }

        public Task<ActionResponse> Login()
        {
            return null;
        }

        public Task<ActionResponse> Logout()
        {
            return null;
        }

        public Task<ActionResponse> Post(string title, string data)
        {
            return null;
        }
    }
}
