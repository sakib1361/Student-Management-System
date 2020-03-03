using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using System;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;
        private readonly IPlatformService _platformService;
        public bool StepBox1 => RegistrationState >= (int)RegistrationState.Roll;
        public bool StepBox2 => (int)RegistrationState >= (int)RegistrationState.Mobile;
        public bool StepBox3 => (int)RegistrationState >= (int)RegistrationState.Password;
        public bool ShowRoll => RegistrationState == RegistrationState.Roll;
        public bool ShowMobile => RegistrationState == RegistrationState.Mobile;
        public bool ShowPassword => RegistrationState == RegistrationState.Password;

        public RegistrationState RegistrationState { get; private set; }
        public string OTPToken { get; set; }
        public string PhoneNo { get; set; }
        public string RollNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private string VerificationId;

        public RegistrationViewModel(IMemberHandler memberHandler, IPlatformService platformService)
        {
            _memberHandler = memberHandler;
            _platformService = platformService;
            RollNo = " ";
            PhoneNo = "+88";
        }

        public ICommand PhoneSubmitCommand => new RelayCommand(PhoneSubmitAction);
        ICommand CodeSentCommand => new RelayCommand<string>(CodeSentAction);
        ICommand VerifyOtpCommand => new RelayCommand<string>(VerifyComplete);
        ICommand VerifyFailedCommand => new RelayCommand<string>(VerifyFailed);
        ICommand VerifyAuthCommand => new RelayCommand(() => RegistrationState = RegistrationState.Password);
        public ICommand VerifyCommand => new RelayCommand(VerifyAction);
        public ICommand RegisterCommand => new RelayCommand(RegisterAction);

        private async void RegisterAction()
        {
            RollNo = RollNo.Trim();
            PhoneNo = PhoneNo.Trim();
            if (string.IsNullOrWhiteSpace(Password))
            {
                _dialog.ShowMessage("Error", "Invalid Password");
            }
            else if (Password.Length < 8)
            {
                _dialog.ShowMessage("Error", "Password must be at least 8digit");
            }
            else if (Password != ConfirmPassword)
            {
                _dialog.ShowMessage("Error", "Password does not match");
            }
            else
            {
                var res = await _memberHandler.Register(RollNo, PhoneNo, Password);
                if (res != null)
                {
                    _dialog.ShowToastMessage(res.Message);
                    _nav.GoBack();
                }
            }
        }

        private async void VerifyAction()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(VerificationId)
                || string.IsNullOrWhiteSpace(OTPToken))
            {
                _dialog.ShowMessage("Error", "Invalid Token");
            }
            else
            {
                var res = await _platformService.VerifyOTP(VerificationId, OTPToken);
                IsBusy = false;
                if (res)
                {
                    RegistrationState = RegistrationState.Password;
                }
            }
        }

        private async void PhoneSubmitAction()
        {
            if (string.IsNullOrWhiteSpace(RollNo))
            {
                _dialog.ShowToastMessage("Invalid Roll Number");
            }
            else if (string.IsNullOrWhiteSpace(PhoneNo))
            {
                _dialog.ShowToastMessage("Invalid Mobile Number");
            }
            var response = await _memberHandler.VerifyPhoneNo(RollNo, PhoneNo);
            if (response != null && response.Actionstatus)
            {
                RegistrationState = RegistrationState.Mobile;
                try
                {
                    _platformService.VerifyPhoneNumber(PhoneNo,
                        VerifyOtpCommand, VerifyFailedCommand, CodeSentCommand, VerifyAuthCommand);
                }
                catch (Exception ex)
                {
                    LogEngine.Error(ex);
                }
            }
            else
            {
                _dialog.ShowMessage("Error", response?.Message);
            }
        }

        private void VerifyComplete(string smsCode)
        {
            OTPToken = smsCode;
        }

        private void VerifyFailed(string message)
        {
            IsBusy = false;
            _dialog.ShowMessage("Error", message);
            if (string.IsNullOrWhiteSpace(OTPToken))
            {
                RegistrationState = RegistrationState.Roll;
            }
        }

        private void CodeSentAction(string verifyId)
        {
            VerificationId = verifyId;
        }
    }
    public enum RegistrationState
    {
        Roll,
        Mobile,
        Password
    }
}
