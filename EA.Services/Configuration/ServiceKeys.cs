namespace EA.Services.Configuration;

public class ServiceKeys
{
    public Servicekey? Keys { get; set; }

    public class Servicekey
    {
        public string? FirstAdminPass { get; set; }
        public string? JwtSecretKey { get; set; }
    }
}