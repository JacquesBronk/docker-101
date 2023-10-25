using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWriteToSql;
using Testcontainers.MsSql;

namespace TestAutomation.Examples.ConfigurationProviders;

/// <summary>
/// ConnectionString Helper
/// </summary>
public class GoodSqlConfigProvider : IConfigurationProvider
{
    private readonly MsSqlContainer _sqlContainer;

    public GoodSqlConfigProvider(MsSqlContainer sqlContainer)
    {
        _sqlContainer = sqlContainer;
    }


    public ServiceCollection GetConfiguration()
    {
        List<KeyValuePair<string, string>> configValue = new List<KeyValuePair<string, string>>();
        //Inject your config here
        configValue.Add(new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", GetConnectionStringForSqlServer(useDefaults: false)));

        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(configValue!).Build();
        var services = new ServiceCollection();
        // Inject Services here.

        services.AddSimpleSql(config);
        return services;
    }

    private string GetConnectionStringForSqlServer(bool useDefaults = false)
    {
        if (useDefaults)
        {
            return _sqlContainer.GetConnectionString();
        }

        var connectionString = _sqlContainer.GetConnectionString();
        return UpdateConnectionStringDatabaseToTestDb(connectionString);
    }

    private string UpdateConnectionStringDatabaseToTestDb(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        builder.InitialCatalog = "testDb";
        return builder.ConnectionString; 
    }
}