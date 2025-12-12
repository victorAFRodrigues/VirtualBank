using Dapper;

namespace VirtualBank.Infrastructure.Services;

public class MigrationService
{
    private readonly string _migrationFolder;
    private readonly SqliteService _sqliteService; 

    public MigrationService(string migrationFolder, string connectionString)
    {
        _migrationFolder = migrationFolder;
        _sqliteService = new SqliteService(connectionString);
    }
    
    public void RunMigrations()
    {
        using var conn = _sqliteService.Connection();
        conn.Open();
        
        conn.Execute(@"
            CREATE TABLE IF NOT EXISTS __MIGRATIONS (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FileName TEXT NOT NULL UNIQUE,
                AppliedAt TEXT NOT NULL
            );
        ");

        var applied = conn.Query<string>("SELECT FileName FROM __MIGRATIONS").ToHashSet();

        foreach (var file in Directory.GetFiles(_migrationFolder, "*.sql"))
        {
            var fileName = Path.GetFileName(file);

            if (applied.Contains(fileName))
                continue;

            Console.WriteLine($"Running migration: {fileName}");

            var sql = File.ReadAllText(file);

            conn.Execute(sql);

            conn.Execute(
                "INSERT INTO __MIGRATIONS (FileName, AppliedAt) VALUES (@file, datetime('now'))",
                new { file = fileName }
            );

            Console.WriteLine($"Migration applied: {fileName}");
        }
    }
}
