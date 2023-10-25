using Testcontainers.Redis;
using Xunit;

namespace TestAutomation.Examples.Fixtures;

/// <summary>
/// Xunit Feature to handle Db setup and teardown.
/// https://www.danclarke.com/cleaner-tests-with-iasynclifetime
/// </summary>
public class RedisDockerFixture : IAsyncLifetime
{
    private ConfigHelper _configHelper = null!;

    public async Task InitializeAsync()
    {
        // Start Docker container
        await ContainerHelper.StartRedisContainer();
        _configHelper = new(ContainerHelper.RedisContainer);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await ContainerHelper.StopRedisContainer(cancellationToken);
    }

    //Here we do the DI magic.
    //If you've made it this far you probably know what you're doing.
    //Just one thing to keep in mind, resource management is important.
    //Think of your environment teardown and remember stoppages and cleanups.
    //SQL TAKEEEEESSS FOREEEEVER to boot, so leave it running if you can. Use the same container for all tests.
    //Hints: Use ResetRedisDataAsync to reset your data.
    public async Task<ConfigHelper> GetConfigHelperAsync(bool getBadConfig = false)
    {
        if (getBadConfig)
        {
            _configHelper = new(ContainerHelper.RedisContainer, true);
        }

        return await Task.FromResult(_configHelper);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ContainerHelper.StartRedisContainer(cancellationToken);
    }

    public async Task DisposeAsync()
    {
        await ContainerHelper.DisposeRedisContainer();
    }

    // Assuming you have a method to reset Redis data.
    public async Task ResetRedisDataAsync(CancellationToken cancellationToken)
    {
        await ContainerHelper.ResetRedisDatabase(cancellationToken);
    }

    public RedisContainer RedisContainer => ContainerHelper.RedisContainer;
}