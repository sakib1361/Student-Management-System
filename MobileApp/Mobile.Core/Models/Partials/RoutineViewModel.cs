using CoreEngine.Model.DBModel;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.Models.Partials
{
    public class RoutineViewModel : NotifyModel
    {
        public List<Activity> Activities { get; private set; }
        public List<Routine> Routines { get; private set; }
        //private readonly Routine CurrentRoutine;

        public RoutineViewModel()
        {
            Routines = new List<Routine>()
            {
                new Routine(DayOfWeek.Saturday),
                new Routine(DayOfWeek.Sunday),
                new Routine(DayOfWeek.Monday),
                new Routine(DayOfWeek.Tuesday),
                new Routine(DayOfWeek.Wednesday),
                new Routine(DayOfWeek.Thursday),
                new Routine(DayOfWeek.Friday)
            };
        }

        public void SelectRoutine(Routine routine)
        {
            foreach (var item in Routines)
            {
                item.IsSelected = item == routine;
                if (item.IsSelected)
                {
                    Activities = item.Activities;
                }
            }
        }

        internal void Update(List<Activity> allActivity)
        {
            foreach (var item in Routines)
            {
                var dayActivity = allActivity.Where(x => x.DayOfWeek == item.DayOfWeek)
                                             .OrderBy(x => x.TimeOfDay)
                                             .ToList();
                if (dayActivity.Count > 0)
                {
                    item.Activities = dayActivity;
                    if (item.DayOfWeek == DateTime.Now.DayOfWeek)
                    {
                        Activities = dayActivity;
                    }
                }
            }
            SelectRoutine(Routines.FirstOrDefault(x => x.DayOfWeek == DateTime.Today.DayOfWeek));
        }
    }

    public class Routine : NotifyModel
    {
        public DayOfWeek DayOfWeek { get; private set; }

        public Routine(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
            Day = DayOfWeek.ToString().Substring(0, 3);
            Activities = new List<Activity>();
            IsSelected = DayOfWeek == DateTime.Now.DayOfWeek;
        }

        public string Day { get; set; }
        public bool IsSelected { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
