using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class SemesterViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;

        public List<Course> Courses { get; set; }
        public Semester CurrentSemester { get; private set; }
        public SemesterViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args.Length > 0 && args[0] is Semester semester)
            {
                Courses = semester.Courses?.ToList();
                LoaaLessonsAsync();
                CurrentSemester = semester;
            }
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            Courses = await _courseHandler.GetSemesterCourses(CurrentSemester.Id);
            IsRefreshisng = false;
            LoaaLessonsAsync();
        }

        private async void LoaaLessonsAsync()
        {
            if (Courses == null)
            {
                return;
            }

            foreach (var item in Courses)
            {
                var fullCourse = await _courseHandler.GetCourse(item.Id);
                if (fullCourse != null)
                {
                    item.Lessons = fullCourse.Lessons;
                    item.NotifyChange(nameof(Course.Lessons));
                }
            }
        }

        public ICommand CourseCommand => new RelayCommand<Course>(CourseAction);
        public ICommand AddCommand => new RelayCommand(AddAction);

        private void AddAction()
        {
            _nav.NavigateTo<AddUpdateCourseViewModel>(CurrentSemester);
        }

        private void CourseAction(Course obj)
        {
            obj.Semester = CurrentSemester;
            _nav.NavigateTo<CourseDetailViewModel>(obj);
        }
    }
}
