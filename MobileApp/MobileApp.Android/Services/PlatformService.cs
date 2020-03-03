using Android.App;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Messaging;
using Java.Util.Concurrent;
using Mobile.Core.Engines.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileApp.Droid.Services
{
    internal class PlatformService : IPlatformService
    {
        private readonly MainActivity mainActivity;

        public PlatformService(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OpenToast(string text)
        {
            Toast.MakeText(Application.Context, text, ToastLength.Short).Show();
        }
        public void SubsubcribeTopics(params string[] topics)
        {
            foreach (var item in topics)
            {
                FirebaseMessaging.Instance.SubscribeToTopic(item.ToLower().Replace(" ", ""));
            }
        }

        public void UnsubscribeTopics(params string[] topics)
        {
            foreach (var item in topics)
            {
                FirebaseMessaging.Instance.UnsubscribeFromTopic(item.ToLower().Replace(" ", ""));
            }
        }

        public void VerifyPhoneNumber(string mobile,
            ICommand onComplete,
            ICommand onFailed, ICommand codeSent, ICommand onverified)
        {
            var phoneAuthCallbacks = new PhoneAuthCallbacks(onComplete, onFailed, codeSent, onverified);
            PhoneAuthProvider.Instance
                .VerifyPhoneNumber(mobile, 60, TimeUnit.Seconds,
                mainActivity,
                phoneAuthCallbacks);

        }

        public async Task<bool> VerifyOTP(string verificationId, string otp)
        {
            var credential = PhoneAuthProvider.GetCredential(verificationId, otp);
            var res = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);
            return res.User != null;
        }
    }

    public class PhoneAuthCallbacks : PhoneAuthProvider.OnVerificationStateChangedCallbacks
    {
        private readonly ICommand onOTPComplete;
        private readonly ICommand onFailed;
        private readonly ICommand onSent;
        private readonly ICommand onVerifyComplete;

        private bool onSMS;

        public PhoneAuthCallbacks()
        {

        }
        public PhoneAuthCallbacks(ICommand onComplete, ICommand onFailed, ICommand onSent, ICommand onVerifyComplete)
        {
            onOTPComplete = onComplete;
            this.onFailed = onFailed;
            this.onSent = onSent;
            this.onVerifyComplete = onVerifyComplete;
        }

        public override async void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            if (onSMS)
            {
                onOTPComplete?.Execute(credential.SmsCode);
            }
            else
            {
                var res = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);
                if (res.User != null)
                {
                    onVerifyComplete?.Execute(null);
                }
            }

        }

        public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
        {
            // The SMS verification code has been sent to the provided phone number, we
            // now need to ask the user to enter the code and then construct a credential
            // by combining the code with a verification ID.
            onSMS = true;
            onSent?.Execute(verificationId);
            base.OnCodeSent(verificationId, forceResendingToken);
        }

        public override void OnVerificationFailed(FirebaseException p0)
        {
            onFailed?.Execute(p0.Message);
        }

        public override void OnCodeAutoRetrievalTimeOut(string p0)
        {
            base.OnCodeAutoRetrievalTimeOut(p0);
            onFailed?.Execute(p0);
        }
    }
}