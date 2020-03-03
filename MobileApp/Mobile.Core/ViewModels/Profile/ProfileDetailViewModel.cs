using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class ProfileDetailViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;

        public DBUser CurrentUser { get; private set; }
        public List<Semester> Semesters { get; set; }
        public bool EditPassword { get; private set; }
        public bool EditProfile { get; private set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ProfileDetailViewModel(IMemberHandler memberHandler)
        {
            _memberHandler = memberHandler;
        }
        public override void OnAppear(params object[] args)
        {
            CurrentUser = AppService.CurrentUser;
        }

        public ICommand AllowPasswordEditCommand => new RelayCommand(() => EditPassword = true);
        public ICommand AllowProfileEditCommand => new RelayCommand(() => EditProfile = true);
        public ICommand EditPasswordCommand => new RelayCommand(EditPasswordAction);
        public ICommand EditProfileCommand => new RelayCommand(EditProfileAction);


        private async void EditProfileAction()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.PhoneNumber))
            {
                _dialog.ShowMessage("Error", "Invalid Mobile Number");
            }
            else
            {
                var resp = await _memberHandler.UpdateUser(CurrentUser);
                if (resp != null)
                {
                    EditProfile = false;
                    _dialog.ShowToastMessage(resp.Message);
                }
            }
        }

        private async void EditPasswordAction()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                _dialog.ShowMessage("Error", "Invalid Password");
            }
            else if (string.IsNullOrWhiteSpace(NewPassword))
            {
                _dialog.ShowMessage("Error", "Invalid New password");
            }
            else if (NewPassword.Length < 8)
            {
                _dialog.ShowMessage("Error", "Password must be at least 8 digit length");
            }
            else if (NewPassword != ConfirmPassword)
            {
                _dialog.ShowMessage("Error", "Password does not match");
            }
            else
            {
                EditPassword = false;

                var resp = await _memberHandler.ChangePassword(CurrentPassword, NewPassword);
                ShowResponse(resp);
            }
        }
    }
}
