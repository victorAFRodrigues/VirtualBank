using Microsoft.Data.Sqlite;
using System.Data;

namespace VirtualBank.Infrastructure.Services;

public class SqliteService
{
    private readonly string _dbPath;

    public SqliteService(string dbPath)
    {
        _dbPath = dbPath;
    }

    public IDbConnection Connection() => new SqliteConnection($"Data Source={_dbPath}");
}