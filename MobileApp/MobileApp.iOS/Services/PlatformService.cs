using Firebase.Auth;
using Firebase.CloudMessaging;
using Foundation;
using Mobile.Core.Engines.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using UIKit;

namespace MobileApp.iOS.Services
{
    class PlatformService : IPlatformService
    {
        const double SHORT_DELAY = 2;

        NSTimer alertDelay;
        UIAlertController alert;

        public void OpenToast(string text)
        {
            ShowAlert(text, SHORT_DELAY);
        }

        public async void SubsubcribeTopics(params string[] topics)
        {
            foreach (var item in topics)
            {
                var topic = item.ToLower().Replace(" ", "");
                await Messaging.SharedInstance.SubscribeAsync(topic);
            }
        }

        public async void UnsubscribeTopics(params string[] topics)
        {
            foreach (var item in topics)
            {
                var topic = item.ToLower().Replace(" ", "");
                await Messaging.SharedInstance.UnsubscribeAsync(topic);
            }
        }

        public Task<bool> VerifyOTP(string verificationId, string otp)
        {
            return Task.FromResult(false);
        }

        public void VerifyPhoneNumber(string mobile, ICommand onComplete, ICommand onFailed, ICommand codeSent, ICommand verifyAuthCommand)
        {
            var result = new VerificationResultHandler(DataComplete);
            PhoneAuthProvider.DefaultInstance.VerifyPhoneNumber(mobile, null, result);
        }

        private void DataComplete(string verificationId, NSError error)
        {
           
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                DismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        void DismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}