using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Batch
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        [Display(Name = "Number of Semester")]
        public int NumberOfSemester { get; set; }
        /// <summary>
        /// In Months
        /// </summary>
        [Required]
        [Range(1, 100)]
        [Display(Name = "Semester duration in months")]
        public int SemesterDuration { get; set; } = 1;
        public ICollection<DBUser> Students { get; set; }
        public ICollection<Semester> Semesters { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartsOn { get; set; }
        public DateTime EndsOn { get; set; }

        public Batch()
        {
            Students = new HashSet<DBUser>();
            Semesters = new HashSet<Semester>();
        }
    }
}
