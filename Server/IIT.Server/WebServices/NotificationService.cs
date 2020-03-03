using CoreEngine.Engine;
using Newtonsoft.Json.Linq;
using Student.Infrastructure.AppServices;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IIT.Server.WebServices
{
    public class NotificationService : INotificationService
    {
        private readonly string GCMUrl;
        private readonly string GCMKey;

        public NotificationService(NotificationSenderOption notificationSenderOption)
        {
            GCMUrl = notificationSenderOption.GCMUrl;
            GCMKey = notificationSenderOption.GCMKey;
        }
        
        public async void SendNotification(string topic, string title, string message)
        {
            try
            {
                var notification = new JObject
                {
                    ["body"] = message,
                    ["title"] = title,
                    ["sound"] = "default"
                };
                var topicData = new JObject
                {
                    ["to"] = "/topics/" + topic.ToLower().Replace(" ", ""),
                    ["priority"] = "high",
                    ["notification"] = notification,
                    ["ttl"] = 3600
                };
                var payLoaddata = new StringContent(topicData.ToString(), Encoding.UTF8, "application/json");
               
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GCMKey);
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GCMKey);
#if DEBUG
                await Task.Delay(100);
                Console.WriteLine(string.Format("   Notification demo: {0}=> {1}, {2}",
                                topic, title, message));
#else
                var resp = await httpClient.PostAsync(GCMUrl, payLoaddata);
             
                if(resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine("Notification: " + topic + data.Trim());
                }
               
                Console.WriteLine("Notification:" + resp.StatusCode);
#endif

            }
            catch (Exception ex)
            {
                LogEngine.Error(ex);
            }
        }
    }

    public class NotificationSenderOption
    {
        public string GCMUrl { get; set; } = "https://fcm.googleapis.com/fcm/send";
        public string GCMKey { get; set; } 
    }
}
