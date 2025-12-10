using Dapper;
using Microsoft.Data.Sqlite;

using VirtualBank.Domain.Interfaces;
using VirtualBank.Domain.Entities;

namespace VirtualBank.Infrastructure.Repositories;
class ClientRepository : IClientRepository
    {
        public void Create(Client client)
        {
           using var conn = new SqliteConnection("Filename=:memory:");
        }
        
        public List<Client> Get()
        {   
            var clients = new List<Client>();
        
            return clients;
        }
        public Client? GetOne(string cpf)
        {
            return null;
        }
        public void Update(Client client)
        {
            
        }
        public void Delete(string cpf)
        {
            
        }
    }
