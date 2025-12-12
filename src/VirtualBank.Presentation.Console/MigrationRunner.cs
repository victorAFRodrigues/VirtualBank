using VirtualBank.Domain.Interfaces;

namespace VirtualBank.Presentation.Console;

public class MigrationRunner : IMigrationRunner
{
    private readonly MigrationService _migrationService;

    public MigrationRunner(IMigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    public void Run()
    {
        _migrationService.RunMigrations();
    }
}