namespace PushNotifTelegram.Exceptions
{
    public class CustomException
    {
        public class NotConnectedException : Exception
        {
            public NotConnectedException(string message) : base(message) { }
        }

        public class UserNotFoundException : Exception
        {
            public UserNotFoundException(string message) : base(message) { }
        }

        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }

        public class TooManyRequestsException : Exception
        {
            public TooManyRequestsException(string message) : base(message) { }
        }

    }
}
