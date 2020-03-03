using CoreEngine.APIHandlers;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsPresented { get; set; }

        private readonly IMemberHandler _memberHandler;

        public List<MenuItem> MenuItems { get; private set; }
        public MainViewModel(IMemberHandler memberHandler)
        {
            _memberHandler = memberHandler;
            MenuItems = new List<MenuItem>()
            {
                new MenuItem("Home",IconType.Home,typeof(HomeViewModel)),
                new MenuItem("Courses",IconType.Class,typeof(CoursesViewModel)),
                new MenuItem("Notices",IconType.Notifications,typeof(NoticesViewModel)),
                new MenuItem("Result",IconType.Grade,typeof(GradesViewModel))
            };
        }

        public ICommand FlyoutCommand => new RelayCommand<MenuItem>(FlyoutAction);
        public ICommand LogoutCommand => new RelayCommand(LogoutAction);
        public ICommand StudentsCommand => new RelayCommand(StudentAction);

        private void StudentAction()
        {
            IsPresented = false;
            _nav.NavigateTo<StudentsViewModel>();
        }

        private void LogoutAction()
        {
            IsPresented = false;
            _memberHandler.Logout();
            _nav.Init<LoginViewModel>();
        }

        private void FlyoutAction(MenuItem obj)
        {
            IsPresented = false;
            _nav.NavigateTo(obj.Type);
        }
    }

    public class MenuItem
    {
        public string Title { get; private set; }
        public IconType Icon { get; private set; }
        public Type Type { get; private set; }

        public MenuItem(string title, IconType icon, Type type)
        {
            Title = title;
            Icon = icon;
            Type = type;
        }
    }
}
