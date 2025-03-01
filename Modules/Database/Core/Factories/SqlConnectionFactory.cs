using System.Data;
using System.Data.SqlClient;
using ThePalace.Database.Core.Interfaces;

namespace ThePalace.Database.Core.Factories;

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    ~SqlConnectionFactory() => this.Dispose();

    public void Dispose()
    {
        _connection?.Dispose();
    }

    public string ConnectionString { get; internal set; } = connectionString;
    private IDbConnection _connection;

    public IDbConnection GetOpenConnection()
    {
        if (_connection?.State != ConnectionState.Open)
        {
            _connection?.Dispose();

            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
        }

        return _connection;
    }

    public IDbConnection CreateNewConnection()
    {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();

        return connection;
    }

    public string GetConnectionString() =>
        ConnectionString;
}