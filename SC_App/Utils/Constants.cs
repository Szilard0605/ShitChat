namespace SC_App.Utils
{
    public class Constants
    {
        public const string IPV4_REGEX = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        public const int MAX_NICKNAME_CHARS = 7;


        public class StatusMessages
        {
            public const string BLANK_FIELD = "Fields cannot be blank!";
            public const string INVALID_ADDRESS_FORMAT = "Ip address is not in a valid format!";

            public class Host
            {
                public const string MAX_CLIENTS_LOW = "Max clients cannot be 0 or lower!";
                public const string DUPLICATE_SERVER = "Theres already a server created with that ip address and port!";
                public const string UNEXPECTED_ERROR = "Unexpected error, couldn't create server.";
            }
            public class Connect
            {
                public const string LONG_NICKNAME = "Nickname cannot be longer than 7 characters";
                public const string DUPLICATE_SERVER = "You are already connected to a server with this ip and port";
                public const string UNEXPECTED_ERROR = "Unexpected error, couldn't connect to server.";
            }
        }
    }
}
