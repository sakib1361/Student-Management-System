using CoreEngine.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEngine.Model.DBModel
{
    public class DBUser : DBNotifyModel
    {
        public virtual Batch Batch { get; set; }
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }


        public string Name { get; set; }
        public bool ClassRepresentative { get; set; }
        public int Roll { get; set; }
        public string Role { get; set; }
        public DateTime EnrolledIn { get; set; }
        public bool RequirePasswordChange { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string WorkHistory { get; set; }
        public string Occupation { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public void UpdateUser(DBUser user)
        {
            Name = user.Name;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Occupation = user.Occupation;
            Address = user.Address;
            BloodGroup = user.BloodGroup;
            WorkHistory = user.WorkHistory;
        }

        [NotMapped]
        [JsonIgnore]
        public bool IsChecked { get; set; }
    }
}
