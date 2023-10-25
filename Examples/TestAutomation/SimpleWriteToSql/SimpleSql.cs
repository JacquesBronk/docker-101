using Microsoft.Data.SqlClient;

namespace SimpleWriteToSql;

public class SimpleSql: ISimpleSql
{
    private readonly string _connectionString;
    
    public SimpleSql(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task WriteToSqlAsync(string message)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO [dbo].[Messages] ([Message]) VALUES (@message)";
        command.Parameters.AddWithValue("@message", message);
        await command.ExecuteNonQueryAsync();
    }
    
    public async Task<List<string>> GetMessagesAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT [Message] FROM [dbo].[Messages]";
        var messages = new List<string>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            messages.Add(reader.GetString(0));
        }
        return messages;
    }
    
    
}