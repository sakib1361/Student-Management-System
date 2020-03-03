using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CoreEngine.Model.DBModel
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime EventTime { get; set; }
        public string OwnerId { get; set; }
        public List<DBUserTodoItem> Participents { get; set; }
        [NotMapped]
        public List<string> ParticementUserIds { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string OwnerName { get; set; }
        [JsonIgnore]
        [NotMapped]
        public string AllParticipents { get; set; }

        private DBUser GetOwnerData()
        {
            if (OwnerId == null || Participents == null) return null;
            var owner = Participents.FirstOrDefault(x => x.DBUser.Id == OwnerId);
            return owner?.DBUser;
        }

        public void Update()
        {
            var owner = GetOwnerData();
            if (owner != null)
            {
                OwnerName = owner.Name;
                Participents.Remove(Participents.FirstOrDefault(x=>x.DBUser == owner));
            }
            AllParticipents = string.Join(",", Participents.Select(x => x.DBUser.Name));
        }
    }

    public class DBUserTodoItem
    {
        public int Id { get; set; }
        [Required]
        public virtual DBUser DBUser { get; set; }
        [Required]
        public virtual ToDoItem ToDoItem { get; set; }
     }
}
