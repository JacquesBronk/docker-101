using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleWriteToSql;
using TestAutomation.Examples.Fixtures;
using Xunit;

namespace TestAutomation.Examples.DatabaseTests;

public class SqlDatabaseTests:  IClassFixture<SqlServerDockerFixture>
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly SqlServerDockerFixture _sqlServerDockerFixture;

    public SqlDatabaseTests(SqlServerDockerFixture sqlServerDockerFixture)
    {
        _sqlServerDockerFixture = sqlServerDockerFixture;
    }
    
    [Fact]
    public async Task SqlServerDatabaseTest()
    {
        //Arrange
        var configHelper = await _sqlServerDockerFixture.GetConfigHelperAsync();
        var sql = configHelper.ServiceProvider.GetRequiredService<ISimpleSql>();
        
        Bogus.Faker faker = new();
        
        //Act
        await sql.WriteToSqlAsync(faker.Lorem.Sentence());
        
        //Assert
        var messages = await sql.GetMessagesAsync();
        messages.Should().HaveCountGreaterThan(0);
    }
}