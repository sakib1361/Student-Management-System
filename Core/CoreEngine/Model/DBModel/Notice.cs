using CoreEngine.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CoreEngine.Model.DBModel
{
    public class Notice : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public PostType PostType { get; set; }
        public bool FutureNotification { get; set; } = true;
        public DateTime CreatedOn { get; set; }
        public DateTime EventDate { get; set; }
        public ICollection<DBFile> DBFiles { get; set; }

        //These parameters are optional and can be used as accessory
        public virtual Batch Batch { get; set; }
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public virtual DBUser Owner { get; set; }
        public Notice()
        {
            DBFiles = new HashSet<DBFile>();
            CreatedOn = CurrentTime;
            EventDate = CurrentTime;
        }
        [NotMapped]
        [JsonIgnore]
        public string TimeOfEvent => GetNoticeEvent();
        [NotMapped]
        [JsonIgnore]
        public string NameType => PostType.ToString()[0].ToString();

        private string GetNoticeEvent()
        {
            return EventDate.ToString("hh:mm tt");
        }
    }

    public enum PostType
    {
        Notice,
        Examination,
        ClassCancel,
        ExtraClass,
        All
    }
}
