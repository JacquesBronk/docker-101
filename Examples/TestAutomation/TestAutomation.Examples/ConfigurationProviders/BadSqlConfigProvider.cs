using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWriteToSql;

namespace TestAutomation.Examples.ConfigurationProviders;


/// <summary>
/// Negative Test Simulation. We know the connection is Faulty.
/// </summary>
public class BadSqlConfigProvider: IConfigurationProvider
{

    public ServiceCollection GetConfiguration()
    {
        List<KeyValuePair<string, string>> configValue = new List<KeyValuePair<string, string>>();
        //Inject your config here
        configValue.Add(new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", $"Server=localhost,1234;Database=testDb;User Id=sa;Password=;Connection Timeout=2"));

        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(configValue!).Build();
        var services = new ServiceCollection();
        // Inject Services here.
        services.AddSimpleSql(config);
        return services;
    }
}