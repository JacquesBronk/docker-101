using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWriteToRedis;
using Testcontainers.Redis;

namespace TestAutomation.Examples.ConfigurationProviders;

/// <summary>
/// ConnectionString Helper
/// </summary>
public class GoodRedisConfigProvider: IConfigurationProvider
{
    private readonly RedisContainer _redisContainer;

    public GoodRedisConfigProvider(RedisContainer redisContainer)
    {
        _redisContainer = redisContainer;
    }

    public ServiceCollection GetConfiguration()
    {
        List<KeyValuePair<string, string>> configValue = new List<KeyValuePair<string, string>>();
        //Inject your config here
        configValue.Add(new KeyValuePair<string, string>("ConnectionStrings:Redis", $"localhost:{_redisContainer.GetMappedPublicPort(6379)},defaultDatabase=1,abortConnect=false,connectTimeout=100,asyncTimeout=500,syncTimeout=500,connectRetry=3,keepAlive=2,allowAdmin=true,ssl=false"));
        
        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(configValue!).Build();
        var services = new ServiceCollection();
        
        services.AddSimpleRedis(config);
        return services;
    }
}