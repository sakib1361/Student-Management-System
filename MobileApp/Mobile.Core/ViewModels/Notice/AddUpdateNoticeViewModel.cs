using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateNoticeViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        private readonly ICourseHandler _courseHandler;

        public Notice CurrentNotice { get; private set; }
        public IEnumerable<PostType> PostTypes { get; set; }
        public PostType CurrentPost { get; set; }
        public ObservableCollection<DBFile> DBFiles { get; set; }

        private readonly IPreferenceEngine _preferneceEngine;

        public List<Course> Courses { get; set; }
        public Course CurrentCourse { get; set; }
        public TimeSpan EventTime { get; set; } = TimeSpan.FromHours(9);
        public bool HasCourse { get; set; }

        public AddUpdateNoticeViewModel(INoticeHandler noticeHandler,
            ICourseHandler courseHandler, IPreferenceEngine preferenceEngine)
        {
            _noticeHandler = noticeHandler;
            _courseHandler = courseHandler;
            PostTypes = Enum.GetValues(typeof(PostType)).Cast<PostType>();
            DBFiles = new ObservableCollection<DBFile>();
            _preferneceEngine = preferenceEngine;
        }

        public override async void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            var semesters = await _courseHandler.GetStudentCurrentSemesters();
            Courses = semesters.SelectMany(x => x.Courses).ToList();
            if (args.Length > 0 && args[0] is Notice notice)
            {
                CurrentNotice = notice;
            }
            else if (args.Length == 1 && args[0] is PostType postType)
            {
                CurrentNotice = new Notice
                {
                    PostType = postType
                };
            }
            else if (args.Length == 1 && args[0] is Course course)
            {
                CurrentCourse = Courses.FirstOrDefault(x => x.Id == course.Id);
                CurrentNotice = new Notice
                {
                    Title = "Notice for " + CurrentCourse.CourseId
                };
                HasCourse = true;
            }
            else
            {
                CurrentNotice = new Notice();
            }
            CurrentPost = PostTypes.FirstOrDefault(x => x == CurrentNotice.PostType);
        }


        public ICommand SaveCommand => new RelayCommand(SaveAction);
        public ICommand AddMaterialCommand => new RelayCommand(AddMaterialAction);

        private async void AddMaterialAction()
        {
            var pickFile = await _preferneceEngine.PickFile();
            if (pickFile != null)
            {
                DBFiles.Add(pickFile);
            }
        }

        private async void SaveAction()
        {
            if (string.IsNullOrWhiteSpace(CurrentNotice.Title))
            {
                _dialog.ShowMessage("Error", "Invalid Title");
            }
            else if (string.IsNullOrWhiteSpace(CurrentNotice.Message))
            {
                _dialog.ShowMessage("Error", "Invalid Message");
            }
            else
            {
                CurrentNotice.PostType = CurrentPost;
                if (CurrentCourse != null && HasCourse)
                {
                    CurrentNotice.CourseId = CurrentCourse.Id;
                }
                IsBusy = true;
                if (CurrentNotice.Id == 0)
                {
                    var res = await _noticeHandler.AddPost(CurrentNotice, DBFiles.ToList());
                    ShowResponse(res,true);
                }
                else
                {
                    var res = await _noticeHandler.UpdatePost(CurrentNotice);
                    ShowResponse(res,true);
                }
                IsBusy = false;
            }
        }
    }
}
