using static PushNotifTelegram.Exceptions.CustomException;

namespace PushNotifTelegram.Helpers
{
    public class RPCExceptionHelper
    {
        public static void RPCExceptionHandler(TL.RpcException ex)
        {
            Exception exception = ex.Message switch
            {
                var msg when msg.Contains("USERNAME_NOT_OCCUPIED") =>
                    new UserNotFoundException("Username tidak ditemukan"),
                var msg when msg.Contains("PHONE_NOT_OCCUPIED") =>
                    new UserNotFoundException("Nomor telepon tidak terdaftar di Telegram"),
                var msg when msg.Contains("FLOOD_WAIT") =>
                    new TooManyRequestsException("Terlalu banyak permintaan, silakan coba lagi"),
                var msg when msg.Contains("AUTH_KEY_UNREGISTERED") =>
                    new NotConnectedException("Sesi sudah tidak valid, silakan login ulang"),
                _ => ex
            };

            throw exception;
        }
    }
}
