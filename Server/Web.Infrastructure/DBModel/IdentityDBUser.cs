using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using System;

namespace Student.Infrasructure.DBModel
{
    public class IdentityDBUser : IdentityUser
    {
        public DBUser DBUser { get; set; }
        public static IdentityDBUser Create(DBUser dBUser)
        {
            dBUser.Id = Guid.NewGuid().ToString().Replace("-", "");
            return new IdentityDBUser()
            {
                Email = dBUser.Email,
                PhoneNumber = dBUser.PhoneNumber,
                UserName = dBUser.UserName,
                DBUser = dBUser
            };
        }
    }
}
