using VirtualBank.Infrastructure.Services;

class Program
{
    static void Main(string[] args)
    {
        var migrations = new MigrationService();
        migrations.RunMigrations();
    }
}