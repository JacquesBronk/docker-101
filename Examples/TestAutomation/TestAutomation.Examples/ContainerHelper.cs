using System.Text;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace TestAutomation.Examples;

/// <summary>
/// This is used for container testing.
/// NOTE! This is not a real class.
/// This will be static, because we want to instantiate this bad boi as a singleton.
/// PLEASE! Do your own Cleanup, Start/Stop Methods provided. 
/// </summary>
public static class ContainerHelper
{
    private static INetwork _testNet;

    public static RedisContainer RedisContainer { get; private set; } = null;

    public static MsSqlContainer SqlServerContainer { get; private set; } = null;

    public static int RedisPublicPort { get; private set; } = 0;

    public static int SqlServerPublicPort { get; private set; } = 0;

    private static readonly string ServerName = "localhost";

    /// <summary>
    ///  Start the Redis Server Container (create OR start)
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task StartRedisContainer(CancellationToken cancellationToken = default)
    {
        //Check if we have a container, if not, create one because Singleton.
        if (RedisContainer == null)
        {
            _testNet = new NetworkBuilder()
                .Build();

            RedisContainer = new RedisBuilder()
                .WithNetwork(_testNet)
                .WithNetworkAliases(networkAliases: ServerName)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
                .Build();

            await RedisContainer.StartAsync(cancellationToken);

            RedisPublicPort = RedisContainer.GetMappedPublicPort(6379);
        }

        await RedisContainer.StartAsync(cancellationToken);
    }

    /// <summary>
    ///  Start the Sql Server Container (create OR start)
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task StartSqlServerContainer(CancellationToken cancellationToken = default)
    {
        //Check if we have a container, if not, create one create one because Singleton.
        if (SqlServerContainer == null)
        {
            _testNet = new NetworkBuilder()
                .Build();

            SqlServerContainer = new MsSqlBuilder()
                .WithNetwork(_testNet)
                .WithNetworkAliases(networkAliases: ServerName)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MsSqlBuilder.MsSqlPort))
                .Build();


            await SqlServerContainer.StartAsync(cancellationToken);
            SqlServerPublicPort = SqlServerContainer.GetMappedPublicPort(1433);

            //Create the database and tables. This is used for the examples.
            await ExecuteSqlCommandVoidAsync("CREATE DATABASE testDb;", GetConnectionStringForSqlServer(true), cancellationToken);
            await ExecuteSqlCommandVoidAsync("CREATE TABLE Messages (MessageID int IDENTITY(1,1) PRIMARY KEY, Message nvarchar(max) NOT NULL);", GetConnectionStringForSqlServer(), cancellationToken);
            //Create your tables here & generic seeding
            //Bonus Points, make this dynamic.
        }

        await SqlServerContainer.StartAsync(cancellationToken);
    }

    /// <summary>
    ///  If you need to reset the redis Db, this is the method to use.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<bool> ResetRedisDatabase(CancellationToken cancellationToken = default)
    {
        try
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(
                $"{ServerName},{RedisContainer.GetMappedPublicPort(6379)},defaultDatabase=1,abortConnect=false,connectTimeout=100,asyncTimeout=500,syncTimeout=500,connectRetry=3,keepAlive=2,allowAdmin=true,ssl=false");
            var server = redis.GetServer($"{RedisContainer.GetMappedPublicPort(6379)}");
            await server.FlushDatabaseAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    /// <summary>
    /// If you need to reset the sql database, this is the method to use.
    /// </summary>
    /// <param name="valueTuple"></param>
    /// <param name="truncate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<bool> ResetDatabase((string schema, string table) valueTuple, bool truncate = false, CancellationToken cancellationToken = default)
    {
        try
        {
            StringBuilder bob = new();

            string schema = valueTuple.schema;
            string table = valueTuple.table;

            if (truncate)
            {
                bob.Append($"TRUNCATE TABLE [{schema}].[{table}];");
            }
            else
            {
                bob.Append($"DROP TABLE [{schema}].[{table}];");
            }


            await using var connection = new SqlConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new SqlCommand(bob.ToString(), connection);
            await command.ExecuteNonQueryAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    /// <summary>
    /// Get A connection string fo Sql
    /// </summary>
    /// <param name="useDefaults"></param>
    /// <returns></returns>
    public static string GetConnectionStringForSqlServer(bool useDefaults = false)
    {
        string serverNameWithPort;

        if (useDefaults)
        {
            serverNameWithPort = $"{ServerName},{SqlServerContainer.GetMappedPublicPort(MsSqlBuilder.MsSqlPort)}";
            return $"server={serverNameWithPort};user id={MsSqlBuilder.DefaultUsername};password={MsSqlBuilder.DefaultPassword};database={MsSqlBuilder.DefaultDatabase};trustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False;Connection Timeout=2";
        }

        serverNameWithPort = $"{ServerName},{SqlServerPublicPort}";
        return $"server={serverNameWithPort};user id={MsSqlBuilder.DefaultUsername};password={MsSqlBuilder.DefaultPassword};database=testDb;trustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False;Connection Timeout=2";
    }


    /// <summary>
    /// Stop The Redis, can be used to simulate a crash.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task StopRedisContainer(CancellationToken cancellationToken = default)
    {
        await RedisContainer.StopAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///  Stop The SqlServer, can be used to simulate a crash.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task StopSqlServerContainer(CancellationToken cancellationToken = default)
    {
        await SqlServerContainer.StopAsync(cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// Dispose of the Redis Container. Cleanup Tasks
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task DisposeRedisContainer(CancellationToken cancellationToken = default)
    {
        await RedisContainer.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Dispose of the Sql Container. Cleanup Tasks
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task DisposeSqlServerContainer(CancellationToken cancellationToken = default)
    {
        await SqlServerContainer.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// One Way Execution, Don't rely on results.
    /// This is to improve performance.
    /// </summary>
    /// <param name="sqlCommand"></param>
    /// <param name="connectionString"></param>
    /// <param name="cancellationToken"></param>
    private static async Task ExecuteSqlCommandVoidAsync(string sqlCommand, string connectionString,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sqlCommand, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}