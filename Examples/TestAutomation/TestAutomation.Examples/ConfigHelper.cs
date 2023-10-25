using Microsoft.Extensions.DependencyInjection;
using TestAutomation.Examples.ConfigurationProviders;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace TestAutomation.Examples;

public class ConfigHelper
{
    public ServiceProvider ServiceProvider { get; }

    public ConfigHelper(RedisContainer container, bool badConfig = false)
    {
        ServiceProvider = badConfig ? BadRedisConfig(container).BuildServiceProvider() : GoodRedisConfig(container).BuildServiceProvider();
    }
    public ConfigHelper(MsSqlContainer container, bool badConfig = false)
    {
        ServiceProvider = badConfig ? BadSqlServerConfig().BuildServiceProvider() : GoodSqlServerConfig(container).BuildServiceProvider();
    }
    private ServiceCollection GoodRedisConfig(RedisContainer redisContainer) => new GoodRedisConfigProvider(redisContainer).GetConfiguration();
    private ServiceCollection BadRedisConfig(RedisContainer redisContainer) => new BadRedisConfigProvider(redisContainer).GetConfiguration();
    private ServiceCollection GoodSqlServerConfig(MsSqlContainer sqlContainer) => new GoodSqlConfigProvider(sqlContainer).GetConfiguration();
    private ServiceCollection BadSqlServerConfig() => new BadSqlConfigProvider().GetConfiguration();
}