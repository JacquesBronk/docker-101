using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SimpleWriteToRedis;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:Redis"];
        
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString!)); 
        services.AddSingleton<ISimpleRedis, SimpleRedis>();
        return services;
    }
}