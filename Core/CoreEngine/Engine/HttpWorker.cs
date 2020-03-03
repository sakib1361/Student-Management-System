using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoreEngine.Engine
{
    public class HttpWorker
    {
        private readonly HttpClient _httpClient;
        public HttpWorker(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LoggedIn(string sessionKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", sessionKey);
        }

        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private async Task<string> SendRequest(HttpMethod method, string requestPath, HttpContent httpContent)
        {
            try
            {
                HttpResponseMessage msg = null;
                var log = string.Empty;
                if (method == HttpMethod.Get)
                {
                    requestPath += "?";
                    var data = await httpContent.ReadAsStringAsync();
                    requestPath += data;
                    msg = await _httpClient.GetAsync(requestPath);
                }
                else if (method == HttpMethod.Post)
                {
                    msg = await _httpClient.PostAsync(requestPath, httpContent);
                    log = await msg.Content.ReadAsStringAsync();
                }
                else if (method == HttpMethod.Put)
                {
                    msg = await _httpClient.PutAsync(requestPath, httpContent);
                    log = await msg.Content.ReadAsStringAsync();
                }
                if (msg == null)
                {
                    throw new Exception("Invalid Method");
                }
                else if (msg.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(msg.ReasonPhrase + " " + log);
                }
                else
                {
                    var data = await msg.Content.ReadAsStringAsync();
                    return data;
                }
            }
            catch (Exception e)
            {
                LogEngine.Error(e);
                return string.Empty;
            }
        }
        public async Task<T> SendRequest<T>(HttpMethod method, string requestPath, HttpContent httpContent)
        {
            var res = await SendRequest(method, requestPath, httpContent);
            if (string.IsNullOrEmpty(res))
            {
                return default;
            }
            else
            {
                try
                {
                    return await Task.Run(() => JsonConvert.DeserializeObject<T>(res));
                }
                catch (Exception ex)
                {
                    LogEngine.Error(ex);
                    return default;
                }
            }
        }
    }
}
