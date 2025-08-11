using System.Text.RegularExpressions;

namespace PushNotifTelegram.Helpers
{
    public class Validation
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var regex = new Regex(@"^\+[1-9]\d{1,14}$");
            return regex.IsMatch(phoneNumber);
        }
    }
}
