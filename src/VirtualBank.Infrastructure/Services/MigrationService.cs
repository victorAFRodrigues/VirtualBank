using Dapper;

namespace VirtualBank.Infrastructure.Services;

public class MigrationService
{
    private static readonly string MigrationFolder = Path.Combine(AppContext.BaseDirectory, "./Migrations");
    private readonly SqliteService _sqliteService = new();
    
    public void RunMigrations()
    {
        Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Database"));
        
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

        foreach (var file in Directory.GetFiles(MigrationFolder, "*.sql"))
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

            Console.WriteLine($"✓ Migration applied: {fileName}");
        }
    }
}


