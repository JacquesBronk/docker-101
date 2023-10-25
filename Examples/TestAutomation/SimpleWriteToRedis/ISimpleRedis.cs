namespace SimpleWriteToRedis;

public interface ISimpleRedis
{
    Task<bool> SetStringAsync(string key, string value);
    Task<string> GetStringAsync(string key);
}