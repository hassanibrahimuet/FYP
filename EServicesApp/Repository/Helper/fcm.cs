using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Repository.Helper
{
    public class fcm
    {
        private string resend;

        public void sendNotification(string token, string title, string message, int activityId)
        {
            var pushNotification = new PushNotification
            {
                to = token,
                data = new PushNotificationPayLoad
                {
                    ActivityId = activityId,
                    title = title,
                    body = message,
                    IsSoundEnabled = true,
                    NotificationData = null
                }
            };
        }

        public void sendNotification(PushNotification pushNotification)
        {
            string server_api_key = "AIzaSyDkUsQFgBA3u4J3bMOLKmHrxCjr1DdQOBA";
            string sender_id = "714961859577";
            //string deviceToken = token;


            var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", server_api_key));
            tRequest.Headers.Add(string.Format("Sender: id={0}", sender_id));


            var jsonNotificationFormat = JsonConvert.SerializeObject(pushNotification);

            var byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.ContentLength = byteArray.Length;


            using (var dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (var tResponse = tRequest.GetResponse())
                {
                    using (var dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (var tReader = new StreamReader(dataStreamResponse))
                        {
                            var responseFromFirebaseServer = tReader.ReadToEnd();

                            var response = JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                                Console.WriteLine("succeeded");
                            else if (response.failure == 1) Console.WriteLine("failed");
                        }
                    }
                }
            }
        }
    }
}