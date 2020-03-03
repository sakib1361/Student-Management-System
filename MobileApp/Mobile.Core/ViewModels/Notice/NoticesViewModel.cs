using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class NoticesViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        public List<Notice> UpcomingNotices { get; set; }
        public ObservableCollection<Notice> Notices { get; set; }
        private int page = 0;
        private bool canLoadMore;

        public NoticesViewModel(INoticeHandler noticeHandler)
        {
            Notices = new ObservableCollection<Notice>();
            _noticeHandler = noticeHandler;
        }

        public async override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            await LoadUpcomingNotice();

            //RefreshAction();
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            canLoadMore = true;
            page = 0;
            Notices.Clear();
            await LoadNotice();
            IsRefreshisng = false;
        }

        private async Task LoadUpcomingNotice()
        {
            UpcomingNotices = await _noticeHandler.GetUpcomingEvents(1, PostType.All);
        }

        private async Task LoadNotice()
        {
            IsBusy = true;
            var res = await _noticeHandler.GetPosts(page, PostType.All);
            if (res == null)
            {
                canLoadMore = false;
            }
            else
            {
                canLoadMore = res.Count > 5;
                foreach (var item in res)
                {
                    Notices.Add(item);
                }
                page++;
            }
            IsBusy = false;
        }

        public ICommand NoticeCommand => new RelayCommand<Notice>(NoticeAction);
        public ICommand AddCommand => new RelayCommand(AddAction);
        public ICommand LoadMoreCommand => new RelayCommand(LoadMoreAction);

        private void NoticeAction(Notice notice)
        {
            _nav.NavigateTo<NoticeDetailViewModel>(notice);
        }

        private void AddAction()
        {
            _nav.NavigateTo<AddUpdateNoticeViewModel>();
        }
        private async void LoadMoreAction()
        {
            if (canLoadMore && !IsBusy)
            {
               await LoadNotice();
            }
        }
    }
}
