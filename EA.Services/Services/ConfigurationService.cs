using EA.Services.Configuration;
using Newtonsoft.Json;

namespace EA.Services.Services;

public class ConfigurationService
{
    public ServiceKeys? LoadConfiguration()
    {
        //var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.js");
        var configPath = Path.Combine($"D:\\Programming\\OpenSourceDevelopment\\EmployeesAccounting\\EA.ServerGateway\\bin\\Debug\\net6.0", "config.js");
        var jsonContent = File.ReadAllText(configPath);

        var model = JsonConvert.DeserializeObject<ServiceKeys>(jsonContent);
        return model;
    }
}