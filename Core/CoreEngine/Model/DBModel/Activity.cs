using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEngine.Model.DBModel
{
    public class Activity
    {
        public Activity()
        {

        }
        public Activity(string name,
            string description, DayOfWeek dayOfWeek,
            string timeOfDay)
        {
            Name = name;
            DayOfWeek = dayOfWeek;
            TimeOfDay = timeOfDay;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string TimeOfDay { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public ActivityType ActivityType { get; set; }
        public bool HasDescription => !string.IsNullOrEmpty(Description);
    }

    public enum ActivityType
    {
        Lesson,
        Notice,
        Todo
    }
}
