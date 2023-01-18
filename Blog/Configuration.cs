using System.Diagnostics;

namespace Blog
{
    public static class Configuration
    {

        public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3Yjksidjh89qjg-9-w8ajfyurbgaudvnpomjh0aeihj8efjyODdkNDkyYjcwOGU=";

        public static SmtConfiguration Smtp = new();

        public class SmtConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
