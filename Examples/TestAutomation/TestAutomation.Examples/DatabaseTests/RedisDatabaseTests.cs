using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleWriteToRedis;
using TestAutomation.Examples.Fixtures;
using Xunit;

namespace TestAutomation.Examples.DatabaseTests;

public class RedisDatabaseTests: IClassFixture<RedisDockerFixture>
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly RedisDockerFixture _redisDockerFixture;

    public RedisDatabaseTests(RedisDockerFixture redisDockerFixture)
    {
        _redisDockerFixture = redisDockerFixture;
    }
    
    [Fact]
    public async Task RedisDatabaseTests_ShouldBeAbleToConnect()
    {
        //Arrange
        var cancellationToken = _cancellationTokenSource.Token;
        ConfigHelper configHelper = await _redisDockerFixture.GetConfigHelperAsync();
        var redis = configHelper.ServiceProvider.GetRequiredService<ISimpleRedis>();
        
        
        Bogus.Faker faker = new();

        string key = faker.Internet.Random.String2(14, 430);
        string message = faker.Lorem.Sentence();
        
        
        //Act
        await redis.SetStringAsync(key, message);
        
        //Assert
        var cachedMessage = await redis.GetStringAsync(key);
        message.Should().BeEquivalentTo(cachedMessage);
        
    }
}