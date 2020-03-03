using CoreEngine.Model.Common;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface IFacebookService
    {
        bool IsLoggedIn { get; }
        Task<ActionResponse> Login();
        Task<ActionResponse> Logout();
        Task<ActionResponse> Post(string title, string data);
    }
}
