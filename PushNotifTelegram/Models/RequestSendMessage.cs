namespace PushNotifTelegram.Models
{
    public class RequestSendMessage
    {
        public string? username { get; set; } = null;
        public string? phone_number { get; set; } = null;
        public string message { get; set; }
    }
}
