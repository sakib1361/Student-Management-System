using Mobile.Core.Models.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalenderView : ContentView
    {
        private DateTime CurrentMonth;
        public DayItem SelectedItem { get; set; }
        public List<DayItem> DayItems { get; set; }
        public DataTemplate ItemTemplate
        {
            get => MainContainer.ItemTemplate;
            set => MainContainer.ItemTemplate = value;
        }
        public CalenderView()
        {
            InitializeComponent();
            DayItems = new List<DayItem>();
            for (var counter = 0; counter < 35; counter++)
            {
                DayItems.Add(new DayItem());
            }

            CalenderGrid.ItemsSource = DayItems;
            CreateMonth(DateTime.Today);
            var currentDay = DayItems.FirstOrDefault(x => x.DateTime == DateTime.Today);
            SelectItem(currentDay);
        }

        public ActivityCollection ActivityCollection
        {
            get => (ActivityCollection)GetValue(ActivityCollectionProperty);
            set => SetValue(ActivityCollectionProperty, value);
        }
        public static BindableProperty ActivityCollectionProperty =
            BindableProperty.Create(
                nameof(ActivityCollection),
                typeof(ActivityCollection),
                typeof(CalenderView),
                propertyChanged: ActivityCollectionChanged);


        public int Month
        {
            get => (int)GetValue(MonthProperty);
            set => SetValue(MonthProperty, value);
        }
        public static BindableProperty MonthProperty =
            BindableProperty.Create(
                nameof(Month),
                typeof(int),
                typeof(CalenderView),
                 defaultBindingMode: BindingMode.TwoWay);
        public int Year
        {
            get => (int)GetValue(YearProperty);
            set => SetValue(YearProperty, value);
        }
        public static BindableProperty YearProperty =
            BindableProperty.Create(
                nameof(Year),
                typeof(int),
                typeof(CalenderView),
                defaultBindingMode: BindingMode.TwoWay);

        private static void ActivityCollectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalenderView calenderView && newValue is ActivityCollection collection)
            {
                calenderView.Update();
                collection.DataChanged += (s, e) => calenderView.Update();
            }
        }

        private void CreateMonth(DateTime dateTime)
        {
            MonthText.Text = dateTime.ToString("MMMM");
            YearText.Text = dateTime.Year.ToString();


            var start = new DateTime(dateTime.Year, dateTime.Month, 1);
            CurrentMonth = start;
            Month = CurrentMonth.Month;
            Year = CurrentMonth.Year;

            int startDiff = (7 + (start.DayOfWeek - DayOfWeek.Saturday)) % 7;
            start = start.AddDays(-1 * startDiff);
            foreach (var item in DayItems)
            {
                item.Update(start, CurrentMonth);
                if (ActivityCollection == null)
                {
                    item.UpdateCollection(null);
                }
                else
                {
                    item.UpdateCollection(ActivityCollection.GetCollection(item.DateTime));
                }

                start = start.AddDays(1);
            }
        }

        private void Update()
        {
            foreach (var item in DayItems)
            {
                if (ActivityCollection == null)
                {
                    item.UpdateCollection(null);
                }
                else
                {
                    item.UpdateCollection(ActivityCollection.GetCollection(item.DateTime));
                    if (SelectedItem == item)
                    {
                        MainContainer.ItemsSource = item.ItemSource;
                    }
                }
            }
        }

        private bool _locked;
        private void ListView_RightSwiped(object sender, EventArgs e)
        {
            if (_locked)
            {
                return;
            }

            _locked = true;
            CreateMonth(CurrentMonth.AddMonths(1));
            _locked = false;
        }

        private void ListView_LeftSwiped(object sender, EventArgs e)
        {
            if (_locked)
            {
                return;
            }

            _locked = true;
            CreateMonth(CurrentMonth.AddMonths(-1));
            _locked = false;
        }

        private void Date_Selected(object sender, EventArgs e)
        {
            if (sender is Grid frame && frame.BindingContext is DayItem dayItem)
            {
                SelectItem(dayItem);
            }
        }

        private void SelectItem(DayItem dayItem)
        {
            MainContainer.ItemsSource = dayItem.ItemSource;
            SelectedDateText.Text = dayItem.DateTime.ToLongDateString();
            if (dayItem.DateTime.Month != CurrentMonth.Month)
            {
                var selectedDate = dayItem.DateTime;
                CreateMonth(dayItem.DateTime);
                SelectedItem = DayItems.FirstOrDefault(x => x.DateTime == selectedDate);
            }
            else
            {
                SelectedItem = dayItem;
            }

            foreach (var item in DayItems)
            {
                item.Selected(SelectedItem);
            }
        }

        private double ListPosition;
        private async void ArrowUp_Clicked(object sender, EventArgs e)
        {
            ArrowDown.IsVisible = true;
            ArrowUp.IsVisible = false;
            ListPosition = Height - ListContainer.Height;
            await ListContainer.LayoutTo(new Rectangle(0, 0, Width, Height));
        }

        private async void ArrowDown_Clicked(object sender, EventArgs e)
        {
            ArrowUp.IsVisible = true;
            ArrowDown.IsVisible = false;
            await ListContainer.LayoutTo(new Rectangle(0, ListPosition, Width, Height - ListPosition));
        }
    }

    public class DayItem : INotifyPropertyChanged
    {
        public DateTime DateTime { get; set; }
        public string Day => DateTime.Day.ToString();
        public Color BackgroundColor { get; set; } = Color.White;
        public Color ForegroundColor { get; set; } = Color.Black;
        public Color HasItemColor { get; set; } = Color.Transparent;
        public Color BorderColor { get; set; }
        public ICollection ItemSource { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void Update(DateTime actualDate, DateTime currentMonth)
        {
            DateTime = actualDate;
            ForegroundColor = actualDate.Month == currentMonth.Month ? Color.Black : Color.Gray;
            BackgroundColor = Color.White;
            BorderColor = actualDate == DateTime.Today ? Color.Pink : Color.Transparent;
            ItemSource = null;
            NotifyChange(nameof(DateTime));
            NotifyChange(nameof(Day));
            NotifyChange(nameof(ForegroundColor));
            NotifyChange(nameof(BackgroundColor));
            NotifyChange(nameof(BorderColor));
        }

        internal void Selected(DayItem dayItem)
        {
            if (dayItem == null)
            {
                ForegroundColor = Color.Black;
                BackgroundColor = Color.White;
            }
            else if (DateTime == dayItem.DateTime)
            {
                ForegroundColor = Color.White;
                BackgroundColor = Color.Blue;
            }
            else if (DateTime.Month == dayItem.DateTime.Month)
            {
                ForegroundColor = Color.Black;
                BackgroundColor = Color.White;
            }
            else
            {
                ForegroundColor = Color.Gray;
                BackgroundColor = Color.White;
            }
            NotifyChange(nameof(ForegroundColor));
            NotifyChange(nameof(BackgroundColor));
        }

        internal void UpdateCollection(ICollection collection)
        {
            ItemSource = collection;
            if (collection != null && collection.Count > 0)
            {
                HasItemColor = Color.OrangeRed;
            }
            else
            {
                HasItemColor = Color.Transparent;
            }

            NotifyChange(nameof(HasItemColor));
        }
    }
}