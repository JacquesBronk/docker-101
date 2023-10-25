namespace SimpleWriteToSql;

public interface ISimpleSql
{
    Task WriteToSqlAsync(string message);
    Task<List<string>> GetMessagesAsync();
}