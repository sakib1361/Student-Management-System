using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    class MemberEngine : BaseApiEngine, IMemberHandler
    {
        public MemberEngine(HttpWorker httpWorker) : base(httpWorker, "member")
        {
        }

        public Task<ActionResponse> ChangePassword(string currentPassword, string newPassword)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { currentPassword, newPassword });
        }

        public Task<ActionResponse> DeleteUser(DBUser user)
        {
            return SendRequest<ActionResponse>(HttpMethod.Get, user);
        }



        public Task<List<DBUser>> GetCurrentBatchUsers()
        {
            return SendRequest<List<DBUser>>(HttpMethod.Get, null);
        }

        public async Task<SignInResponse> Login(string username, string password)
        {
            var res = await SendRequest<SignInResponse>(HttpMethod.Post, new { username, password });
            if (res != null && res.Success)
            {
                LoginToken(res.Token);
            }
            return res;
        }



        public async void Logout()
        {
            await SendRequest<ActionResponse>(HttpMethod.Get, null);
            LogoutToken();
        }

        public Task<ActionResponse> UpdateUser(DBUser user)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, user);
        }

        public Task<DBUser> TouchLogin()
        {
            return SendRequest<DBUser>(HttpMethod.Get, null);
        }

        public Task<ActionResponse> CreateStudent(int batchId, string roll, string name, string email, string phone)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { batchId, roll, name, email, phone });
        }

        public Task<ActionResponse> CreateBatchStudents(int batchId, DBFile dBFile, IFormFile formFile = null)
        {
            return SendMultiPartRequest<ActionResponse>(new { batchId }, dBFile);
        }

        public Task<List<DBUser>> SearchStudents(string key)
        {
            return SendRequest<List<DBUser>>(HttpMethod.Get, new { key });
        }

        public Task<List<DBUser>> GetCurrentCr()
        {
            return SendRequest<List<DBUser>>(HttpMethod.Get, null);
        }

        public Task<DBUser> GetUser(string userId)
        {
            return SendRequest<DBUser>(HttpMethod.Get, new { userId });
        }

        public Task<ActionResponse> ForgetPassword(string rollNo, string phoneNo, string password)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { rollNo, phoneNo, password });
        }

        public Task<ActionResponse> VerifyPhoneNo(string rollNo, string phoneNo)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { rollNo, phoneNo });
        }

        public Task<ActionResponse> Register(string rollNo, string phoneNo, string password)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { rollNo, phoneNo, password });
        }

        public Task<ActionResponse> MakeCR(string userId, bool isCR)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { userId, isCR });
        }
    }
}
