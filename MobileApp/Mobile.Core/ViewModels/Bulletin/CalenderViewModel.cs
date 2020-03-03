using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Mobile.Core.Models.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.Core.ViewModels
{
    public class CalenderViewModel : BaseViewModel
    {
        public ActivityCollection ActivityCollection { get; set; }
        public List<Activity> Notices { get; set; }

        private readonly INoticeHandler _noticeHandler;

        public DateTime SelectedDate { get; set; }
        public int Year { get; set; } = 2020;
        int _month = 1;
        public int Month
        {
            get => _month;
            set
            {
                _month = value;
                LoadInformation(_month);
            }
        }
        public CalenderViewModel(INoticeHandler noticeHandler)
        {
            _noticeHandler = noticeHandler;
            SelectedDate = DateTime.Now;
            ActivityCollection = new ActivityCollection();
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            ActivityCollection.Clear();
            Year = DateTime.Today.Year;
            _month = DateTime.Today.Month;
            LoadInformation(Month);
        }

        private async void LoadInformation(int month)
        {
            await Task.Delay(100);//Handle year change
            var start = new DateTime(Year, month, 1);
            var end = start.AddMonths(1);
            if (ActivityCollection.RequireData(start, end))
            {
                IsBusy = true;
                Notices = await _noticeHandler.GetActivities(start, end);
                var allData = new Dictionary<DateTime, ICollection>();
                foreach (var group in Notices.GroupBy(x => x.DateTime.Date))
                {
                    allData.Add(group.Key.Date, group.ToList());
                }
                ActivityCollection.AddData(allData);
                IsBusy = false;
            }
        }
    }
}
