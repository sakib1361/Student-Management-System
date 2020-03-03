using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoreEngine.Engine
{
    abstract class BaseApiEngine
    {
        private readonly HttpWorker _httpWorker;
        private readonly string controller;
        private const string api = "api";

        public BaseApiEngine(HttpWorker httpWorker, string Controller)
        {
            _httpWorker = httpWorker;
            controller = Controller;
        }

        protected async Task<T> SendRequest<T>(HttpMethod httpMethod, object data, [CallerMemberName]string member = "")
        {
            //var jsonData = JsonConvert.SerializeObject(data);
            //using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using var content = new FormUrlEncodedContent(await FormHelper.GetPair(data));
            var path = string.Join("/", api, controller, member);
            return await _httpWorker.SendRequest<T>(httpMethod, path, content);
        }

        protected async Task<T> SendMultiPartRequest<T>(object data, List<DBFile> dBFiles, [CallerMemberName]string member = "")
        {
            using var requestContent = new MultipartFormDataContent();
            var jsonData = await FormHelper.GetPair(data);
            var path = string.Join("/", api, controller, member);
            foreach (var item in jsonData)
            {
                requestContent.Add(new StringContent(item.Value), item.Key);
            }

            if (dBFiles != null)
            {
                foreach (var dbFile in dBFiles)
                {
                    var streamContent = new StreamContent(dbFile.FileStream);
                    requestContent.Add(streamContent, "formfiles", dbFile.FileName);
                }
            }
            return await _httpWorker.SendRequest<T>(HttpMethod.Post, path, requestContent);
        }

        protected async Task<T> SendMultiPartRequest<T>(object data, DBFile dBFile, [CallerMemberName]string member = "")
        {
            using var requestContent = new MultipartFormDataContent();
            var path = string.Join("/", api, controller, member);
            var jsonData = await FormHelper.GetPair(data);
            foreach (var item in jsonData)
            {
                requestContent.Add(new StringContent(item.Value), item.Key);
            }
            if (dBFile != null)
            {
                var streamContent = new StreamContent(dBFile.FileStream);
                requestContent.Add(streamContent, "formfile", dBFile.FileName);
            }
            return await _httpWorker.SendRequest<T>(HttpMethod.Post, path, requestContent);
        }

        protected void LoginToken(string token)
        {
            _httpWorker.LoggedIn(token);
        }

        protected void LogoutToken()
        {
            _httpWorker.Logout();
        }
    }
}