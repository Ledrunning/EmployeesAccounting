using System.Configuration;

namespace EA.DesktopApp
{
    public class AppConfig
    {
        private const int DefaultTimeout = 1000;
        public string BaseServerUri { get; set; }
        public string ServerPingUri { get; set; }
        public int Timeout { get; set; }
        public int MaxPingAttempts { get; set; }
        public int ServerPingTimeout { get; set; }

        public AppConfig LoadConfiguration()
        {
            return new AppConfig
            {
                BaseServerUri = ConfigurationManager.AppSettings["serverUriString"],
                ServerPingUri = ConfigurationManager.AppSettings["serverPingUri"],
                Timeout = int.TryParse(ConfigurationManager.AppSettings["timeOut"], out var timeout)
                    ? timeout
                    : -1, // or some default value
                MaxPingAttempts = int.TryParse(ConfigurationManager.AppSettings["maxPingAttempts"],
                    out var maxPingAttempts)
                    ? maxPingAttempts
                    : DefaultTimeout, 
                ServerPingTimeout = int.TryParse(ConfigurationManager.AppSettings["serverPingTimeout"],
                    out var serverPingTimeout)
                    ? serverPingTimeout
                    : DefaultTimeout 
            };
        }
    }
}