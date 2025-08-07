namespace PushNotifTelegram.Services
{
    public class TelegramServices
    {
        public readonly HttpClient _httpClient;
        public TelegramServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task SendMessage(string message)
        {
            var _botToken = "8026778612:AAFidn2nPL_M1lOITK5wG5UfkMlSbrArAOU";
            var _chatId = "7248325894";

            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var payload = new Dictionary<string, string>
            {
                { "chat_id", _chatId },
                { "text", message }
            };

            var content = new FormUrlEncodedContent(payload);
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
