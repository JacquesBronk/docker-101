using StackExchange.Redis;

namespace SimpleWriteToRedis;

public class SimpleRedis : ISimpleRedis
{
    private readonly IDatabase _database;

    public SimpleRedis(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<bool> SetStringAsync(string key, string value)
    {
        await _database.StringSetAsync(key, value);
        return true;
    }

    public async Task<string> GetStringAsync(string key)
    {
        return (await _database.StringGetAsync(key))!;
    }
}