using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IMemberHandler
    {
        Task<SignInResponse> Login(string username, string password);
        Task<ActionResponse> ChangePassword(string currentPassword, string newPassword);
        Task<ActionResponse> ForgetPassword(string rollNo, string phoneNo, string password);
        Task<DBUser> TouchLogin();
        void Logout();
        Task<List<DBUser>> GetCurrentBatchUsers();
        Task<ActionResponse> CreateBatchStudents(int batchId, DBFile dBFile, IFormFile formFile = null);
        Task<ActionResponse> CreateStudent(int batchId, string roll, string name, string email, string phone);
        Task<ActionResponse> UpdateUser(DBUser user);
        Task<ActionResponse> DeleteUser(DBUser user);
        Task<List<DBUser>> SearchStudents(string key);
        Task<List<DBUser>> GetCurrentCr();
        Task<DBUser> GetUser(string userId);
        Task<ActionResponse> VerifyPhoneNo(string rollNo, string phoneNo);
        Task<ActionResponse> Register(string rollNo, string phoneNo, string password);
        Task<ActionResponse> MakeCR(string userId, bool isCR);
    }
}
