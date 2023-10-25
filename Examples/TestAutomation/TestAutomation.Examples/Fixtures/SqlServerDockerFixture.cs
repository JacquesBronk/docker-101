using Testcontainers.MsSql;
using Xunit;

namespace TestAutomation.Examples.Fixtures;

/// <summary>
/// Xunit Feature to handle Db setup and teardown.
/// https://www.danclarke.com/cleaner-tests-with-iasynclifetime
/// </summary>
public class SqlServerDockerFixture : IAsyncLifetime
{
    private ConfigHelper _configHelper = null!;

    public async Task InitializeAsync()
    {
        // Start Docker container
        await ContainerHelper.StartSqlServerContainer();
        _configHelper = new(ContainerHelper.SqlServerContainer);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await ContainerHelper.StopSqlServerContainer(cancellationToken);
    }

    //See RedisDockerFixture for more info.
    public async Task<ConfigHelper> GetConfigHelperAsync(bool getBadConfig = false)
    {
        if (getBadConfig)
        {
            _configHelper = new(ContainerHelper.SqlServerContainer, true);
        }

       
        return await Task.FromResult(_configHelper);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ContainerHelper.StartSqlServerContainer(cancellationToken);
    }

    public async Task DisposeAsync()
    {
        await ContainerHelper.DisposeSqlServerContainer();
    }

    public async Task ResetDatabaseAsync((string schema, string table) valueTuple, bool truncate = false,CancellationToken cancellationToken = default)
    {
        await ContainerHelper.ResetDatabase(valueTuple, truncate, cancellationToken);
    }

    public MsSqlContainer SqlServerContainer => ContainerHelper.SqlServerContainer;
}