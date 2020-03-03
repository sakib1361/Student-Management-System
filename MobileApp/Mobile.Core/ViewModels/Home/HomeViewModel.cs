using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using Mobile.Core.Models.Partials;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        public List<Activity> UpcomingEvents { get; set; }
       
        public DBUser User { get; private set; }
        public string Today { get; private set; }
        public string Date { get; private set; }
        public RoutineViewModel RoutineViewModel { get; private set; }

        public HomeViewModel(INoticeHandler postHandler)
        {
            RoutineViewModel = new RoutineViewModel();
            _noticeHandler = postHandler;
            Today = DateTime.Now.DayOfWeek.ToString();
            Date = DateTime.Now.ToString("dd MMMM yyyy");
        }
        public override void OnAppear(params object[] args)
        {
            User = AppService.CurrentUser;
            IsRefreshisng = true;
        }

        protected async override void RefreshAction()
        {
            base.RefreshAction();
            await LoadEvents();
            IsRefreshisng = false;
        }

        private async Task LoadEvents()
        {
            IsBusy = true;
            int startDiff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Saturday)) % 7;
            var  start = DateTime.Today.AddDays(-1 * startDiff);
            var end =start.AddDays(7).AddSeconds(-1);
            UpcomingEvents = await _noticeHandler.GetActivities(start,end);
            RoutineViewModel.Update(UpcomingEvents);
            IsBusy = false;
        }

        public ICommand SelectLessonCommand => new RelayCommand<Lesson>(SelectLessonAction);
        public ICommand SelectNoticeCommand => new RelayCommand<Notice>(SelectNoticeAction);
        public ICommand SelectCourseCommand => new RelayCommand<Course>(SelectCourseAction);
        public ICommand DaySelectCommand => new RelayCommand<Routine>(WeekAction);
        public ICommand ProfileCommand => new RelayCommand(ProfileAction);
        public ICommand CalenderCommand => new RelayCommand(CalenderAction);
        public ICommand TodoItemCommand => new RelayCommand(TodoItemAction);
        public ICommand NoticesCommand => new RelayCommand(NoticesAction);
        public ICommand SwipeLeftCommand => new RelayCommand(SwipeLeftAction);
        public ICommand SwipteRightCommand => new RelayCommand(SwipeRightAction);
        public ICommand ActivityCommand => new RelayCommand<Activity>(ActiovityAction);

        private void ActiovityAction(Activity activity)
        {
            var message = string.Format("{0}\n{1}\n{2}", activity.ActivityType, activity.TimeOfDay, activity.Description);
            _dialog.ShowMessage(activity.Name, message);
        }

        private void SwipeRightAction()
        {

        }

        private void SwipeLeftAction()
        {

        }

        private void NoticesAction()
        {
            _nav.NavigateTo<NoticesViewModel>();
        }

        private void TodoItemAction()
        {
            _nav.NavigateTo<TodoItemsViewModel>();
        }

        private void CalenderAction()
        {
            _nav.NavigateTo<CalenderViewModel>();
        }

        private void ProfileAction()
        {
            _nav.NavigateTo<ProfileDetailViewModel>();
        }

        private void WeekAction(Routine obj)
        {
            RoutineViewModel.SelectRoutine(obj);
        }

        private void SelectCourseAction(Course obj)
        {
            //throw new NotImplementedException();
        }

        private void SelectNoticeAction(Notice obj)
        {
            //throw new NotImplementedException();
        }

        private void SelectLessonAction(Lesson obj)
        {
            //throw new NotImplementedException();
        }
    }
}
