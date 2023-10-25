using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Redis;

namespace TestAutomation.Examples.ConfigurationProviders;


/// <summary>
/// Negative Test Simulation. We know the connection is Faulty.
/// </summary>
public class BadRedisConfigProvider: IConfigurationProvider
{
    private readonly RedisContainer _redisContainer;
    public BadRedisConfigProvider(RedisContainer redisContainer)
    {
        _redisContainer = redisContainer;
    }

    public ServiceCollection GetConfiguration()
    {
        List<KeyValuePair<string, string>> configValue = new List<KeyValuePair<string, string>>();
        //Inject your config here
        configValue.Add(new KeyValuePair<string, string>("ConnectionStrings:Redis", "localhost:1234"));

        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(configValue!).Build();
        var services = new ServiceCollection();
        // Inject Services here.
        return services;
    }
}