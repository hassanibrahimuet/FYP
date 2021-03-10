namespace Repository.Helper
{
    public class PushNotification
    {
        public string to { get; set; }

        public PushNotificationPayLoad data { get; set; }
    }

    public class PushNotificationPayLoad
    {
        public int ActivityId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public bool IsSoundEnabled { get; set; }
        public object NotificationData { get; set; }
    }
}