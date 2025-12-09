namespace VirtualBank.Infrastructure.Services
{
    public class SqliteService
    {
        private readonly string _connectionString = "Data Source=./Database/data.db";

        public SqliteConnection Connection()
        {
            Directory.CreateDirectory("./Database");
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public string Migrate(string migrationFile)
        {
            try
            {
                using var conn = Connection();
                using var cmd = conn.CreateCommand();

                var migrationScript = File.ReadAllText($"Database/migrations/{migrationFile}");

                cmd.CommandText = migrationScript;
                cmd.ExecuteNonQuery();

                // Validar criação das tabelas
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";

                var tables = new List<string>();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }

                if (tables.Count > 0)
                {
                    return $"Migrations executed successfully. Tables created: {tables.Count}";
                }

                return "Migration executed but no tables were created.";
            }
            catch (Exception ex)
            {
                return $"Migration error: {ex.Message}";
            }
        }

    }
}