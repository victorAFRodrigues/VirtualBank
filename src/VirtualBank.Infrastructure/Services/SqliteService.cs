using Microsoft.Data.Sqlite;
using System.Data;

namespace VirtualBank.Infrastructure.Services;

public class SqliteService
{   
    private const string ConnectionString = "Data Source=./Database/data.db";
    public IDbConnection Connection()
    {
        return new SqliteConnection(ConnectionString);

    }
}