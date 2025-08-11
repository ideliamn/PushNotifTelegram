using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushNotifTelegram.Exceptions;
using PushNotifTelegram.Helpers;
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

        private string Config(string what) => what switch
        {
            "api_id" => _config["Telegram:ApiId"],
            "api_hash" => _config["Telegram:ApiHash"],
            "phone_number" => _config["Telegram:PhoneNumber"],
            "verification_code" => ConsoleInput("Masukkan kode OTP dari Telegram: "),
            "password" => ConsoleInput("Masukkan password 2FA: "),
            _ => null
        };

        private static string ConsoleInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public async Task InitAsync()
        {
            if (_isConnected)
            {
                return;
            }
            Console.WriteLine("Connecting Telegram Client...");
            _isConnected = true;
        }

        private async Task EnsureConnectedAsync()
        {
            await InitAsync();
            if (!_isConnected)
            {
                throw new CustomException.NotConnectedException("Telegram tidak terhubung");
            }
        }

        private async Task<User> ResolveUserAsync(string type, string value)
        {
            try
            {
                User user;
                if (type == "username")
                {
                    var result = await _client.Contacts_ResolveUsername(value);
                    user = result.users.Values.OfType<User>().FirstOrDefault();
                }
                else if (type == "phoneNumber")
                {
                    var result = await _client.Contacts_ResolvePhone(value);
                    user = result.users.Values.OfType<User>().FirstOrDefault();
                }
                else
                {
                    throw new ArgumentException("Type harus username atau phoneNumber");
                }

                if (user == null)
                    throw new CustomException.UserNotFoundException($"User {value} tidak ditemukan");

                return user;
            }
            catch (TL.RpcException ex)
            {
                RPCExceptionHelper.RPCExceptionHandler(ex);
                throw;
            }
        }


        public async Task SendMessageToUsernameAsync(string username, string message)
        {
            await EnsureConnectedAsync();
            var user = await ResolveUserAsync("username", username);
            await _client.SendMessageAsync(user, message);
        }

        public async Task SendMessageToPhoneAsync(string phoneNumber, string message)
        {
            await EnsureConnectedAsync();
            var user = await ResolveUserAsync("phoneNumber", phoneNumber);
            await _client.SendMessageAsync(user, message);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
