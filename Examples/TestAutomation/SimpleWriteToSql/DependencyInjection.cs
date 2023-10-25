using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleWriteToSql;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleSql(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddSingleton<ISimpleSql>(new SimpleSql(connectionString));
        return services;
    }
}