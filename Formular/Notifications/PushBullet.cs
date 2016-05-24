using Formular.Helpers;
using Formular.Notifications.Interfaces;
using Formular.Notifications.Models;
using Formular.Notifications.Serializers;
using RestSharp;

namespace Formular.Notifications
{
    public class PushBullet : INotification
    {
        private static string notificationUrl = "https://api.pushbullet.com/";
        public bool SendNotification(Notification notification)
        {
            var authToken = SettingsManager.PushBulletToken;

            if (string.IsNullOrWhiteSpace(authToken))
                return false;

            var client = new RestClient(notificationUrl);

            var request = new RestRequest("v2/pushes", Method.POST);
            request.AddHeader("Access-Token", authToken);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            SimpleJson.CurrentJsonSerializerStrategy = new CamelCaseSerializer();
            request.AddBody(notification);

            var response = client.Execute(request);
            
            return response.ResponseStatus == ResponseStatus.Completed;
        }
    }
}
