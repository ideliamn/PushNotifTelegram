using TL;
using WTelegram;

namespace PushNotifTelegram.Services
{
    public class TelegramServices
    {
        private readonly Client _client;
        private readonly IConfiguration _config;
        private bool _isConnected = false;
        public TelegramServices(IConfiguration config)
        {
            _config = config;
            _client = new Client(Config);
            Task.Run(async () =>
            {
                await _client.LoginUserIfNeeded();
            });
        }

        private string Config(string what)
        {
            switch (what)
            {
                case "api_id": return _config["Telegram:ApiId"];
                case "api_hash": return _config["Telegram:ApiHash"];
                case "phone_number": return _config["Telegram:PhoneNumber"];
                case "verification_code":
                    Console.Write("Masukkan kode OTP dari Telegram: ");
                    return Console.ReadLine();
                case "password":
                    Console.Write("Masukkan password 2FA: ");
                    return Console.ReadLine();
                default: return null;
            }
        }

        public async Task InitAsync()
        {
            if (!_isConnected)
            {
                await _client.ConnectAsync();
                var me = await _client.LoginUserIfNeeded();
                Console.WriteLine($"Connected as {me.username ?? me.first_name}");
                _isConnected = true;
            }
        }

        public async Task SendMessageToUsernameAsync(string username, string message)
        {
            if (!_isConnected)
            {
                Console.WriteLine("Client belum connect, InitAsync...");
            }

            var user = await _client.Contacts_ResolveUsername(username);
            await _client.SendMessageAsync(user, message);
        }

        public async Task SendMessageToPhoneAsync(string phoneNumber, string message)
        {
            if (!_isConnected) {
                Console.WriteLine("Client belum connect, InitAsync...");
            }

            var result = await _client.Contacts_ResolvePhone(phoneNumber);
            var user = result.users.Values.FirstOrDefault();
            if (user == null)
            {
                throw new Exception($"User dengan nomor {phoneNumber} tidak ditemukan");
            }

            await _client.SendMessageAsync(user, message);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
