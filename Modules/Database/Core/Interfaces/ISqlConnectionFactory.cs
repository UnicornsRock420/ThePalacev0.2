using System.Data;

namespace ThePalace.Database.Core.Interfaces
{
    public interface ISqlConnectionFactory : IDisposable
    {
        string ConnectionString { get; }

        IDbConnection GetOpenConnection();

        IDbConnection CreateNewConnection();
    }
}