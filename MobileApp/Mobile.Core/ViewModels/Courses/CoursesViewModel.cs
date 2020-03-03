using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;

        public List<Semester> Semesters { get; private set; }
        public CoursesViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            IsRefreshisng = true;
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            Semesters = await _courseHandler.GetStudentCurrentSemesters();
            IsRefreshisng = false;
        }

        public ICommand SemesterCommand => new RelayCommand<Semester>(SemesterAction);


        private void SemesterAction(Semester obj)
        {
            _nav.NavigateTo<SemesterViewModel>(obj);
        }
    }
}
