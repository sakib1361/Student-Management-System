using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateLessonViewModel : BaseViewModel, IPopupModel
    {
        private readonly ILessonHandler _lessonHandler;

        public Course CurrentCourse { get; private set; }
        public Lesson Lesson { get; private set; }
        public List<DayOfWeek> DayOfWeeks { get; set; } = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();

        public AddUpdateLessonViewModel(ILessonHandler lessonHandler)
        {
            _lessonHandler = lessonHandler;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args.Length == 2 && args[0] is Course course && args[1] is Lesson lesson)
            {
                CurrentCourse = course;
                Lesson = lesson;
            }
            else
            {
                GoBack();
            }
        }

        public ICommand SaveCommand => new RelayCommand(SaveActionAsync);

        public ICommand DataCommand { get; set; }

        private async void SaveActionAsync()
        {
            if (Lesson.TimeOfDay == TimeSpan.Zero)
            {
                _dialog.ShowMessage("Error", "Invalid Time");
            }
            else
            {
                ActionResponse res;
                if (Lesson.Id == 0)
                {
                    res = await _lessonHandler.AddLesson(CurrentCourse.Id, Lesson);
                }
                else
                {
                    res = await _lessonHandler.UpdateLesson(Lesson);
                }

                if (res != null && res.Actionstatus)
                {
                    _dialog.ShowToastMessage("Updated Lesson Successfully");
                    _nav.GoBack();
                }
            }
        }
    }
}
