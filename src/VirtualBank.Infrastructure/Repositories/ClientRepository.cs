using VirtualBank.Infrastructure.Services;

namespace VirtualBank.Infrastructure.Repositories
{
    class ClientRepository: IRepository<ClientDto>
    {
        private readonly SqliteService _db = new SqliteService();
        public void Create(ClientModel client)
        {
            using var conn = _db.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO CLIENTS (CPF, NAME, AGE, PASSWORD) VALUES (@cpf, @password)";
            cmd.Parameters.AddWithValue("@cpf", client.Cpf);
            cmd.Parameters.AddWithValue("@password", client.Password);
            cmd.ExecuteNonQuery();

            Console.WriteLine($"Clitente com CPF: {client.Cpf} foi cadastrado com sucesso!");
        }

        public List<ClientModel> Get()
        {   
            var clients = new List<ClientModel>();

            using var conn = _db.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT CPF, NAME, PASSWORD FROM CLIENTS";
            var reader = cmd.ExecuteReader();
            
            while(reader.Read())
            {
                clients.Add( 
                    new ClientModel
                    {
                        Cpf = reader.GetString(0),
                        Name = reader.GetString(1),
                        Password = reader.GetString(2)
                    }
                );
            }

            return clients;
        }
        public ClientModel? GetOne(string cpf)
        {
            using var conn = _db.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CPF, PASSWORD FROM CLIENTS WHERE CPF = @cpf";
            cmd.Parameters.AddWithValue("@cpf", cpf);
            var reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                return new ClientModel
                {
                    Cpf = reader.GetString(0),
                    Name = reader.GetString(1),
                    Password = reader.GetString(2)
                };
            }
            
            return null;
        }
        public void Update(ClientModel client)
        {
            using var conn = _db.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE CLIENTS SET PASSWORD = @password WHERE CPF = @cpf";
            cmd.Parameters.AddWithValue("@cpf", client.Cpf);
            cmd.Parameters.AddWithValue("@password", client.Password);
            cmd.ExecuteNonQuery();
        }
        public void Delete(string cpf)
        {
            using var conn = _db.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM CLIENTS WHERE CPF = @cpf";
            cmd.Parameters.AddWithValue("@cpf", cpf);
            cmd.ExecuteNonQuery();
            
        }
    }
}
