using System.Data;
using System.Data.SqlClient;
using ThePalace.Database.Core.Interfaces;

namespace ThePalace.Database.Core.Factories;

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    private IDbConnection _connection;

    public void Dispose()
    {
        _connection?.Dispose();

        GC.SuppressFinalize(this);
    }

    public string ConnectionString { get; internal set; } = connectionString;

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

    ~SqlConnectionFactory()
    {
        Dispose();
    }

    public string GetConnectionString()
    {
        return ConnectionString;
    }
}