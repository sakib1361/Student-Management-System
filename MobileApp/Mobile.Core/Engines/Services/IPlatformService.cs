using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.Core.Engines.Services
{
    public interface IPlatformService
    {
        Task<bool> VerifyOTP(string verificationId, string otp);
        void OpenToast(string text);
        void SubsubcribeTopics(params string[] topics);
        void UnsubscribeTopics(params string[] topics);
        void VerifyPhoneNumber(string mobile, ICommand onComplete, ICommand onFailed, ICommand codeSent, ICommand verifyAuthCommand);
    }
}