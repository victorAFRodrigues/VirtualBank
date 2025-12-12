using Microsoft.Extensions.DependencyInjection;
using VirtualBank.Infrastructure.Services;
using VirtualBank.Presentation.Console;

var services = new ServiceCollection();

// 1. Paths
var solutionRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
var dbPath = Path.Combine(solutionRoot, "database", "data.db");
var migrationsPath = Path.Combine(solutionRoot, "database", "migrations");

// 2. Registrar dependências
services.AddSingleton(new SqliteService(dbPath));
services.AddSingleton<IMigrationService>(provider =>
    new MigrationService(migrationsPath, provider.GetRequiredService<SqliteService>())
);

services.AddSingleton<IMigrationRunner, MigrationRunner>();

// 3. Build
var provider = services.BuildServiceProvider();

// 4. Rodar
var runner = provider.GetRequiredService<IMigrationRunner>();
runner.Run();