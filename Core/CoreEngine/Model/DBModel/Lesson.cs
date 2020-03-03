using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEngine.Model.DBModel
{
    public class Lesson
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan TimeOfDay { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string RoomNo { get; set; }
        public string Description { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string TimeOfLesson => DateTime.Today.Add(TimeOfDay).ToString("hh:mm tt");
    }
}
