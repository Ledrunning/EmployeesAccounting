using EA.Services.Configuration;
using Newtonsoft.Json;

namespace EA.Services.Services;

public class ConfigurationService
{
    public ServiceKeysConfig? LoadConfiguration()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        var jsonContent = File.ReadAllText(configPath);

        var model = JsonConvert.DeserializeObject<ServiceKeysConfig>(jsonContent);
        return model;
    }
}