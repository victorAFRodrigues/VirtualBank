# As 4 camadas da Clean Architecture (exemplos em C#)

## 1. Domain (Camada de Domínio)

É o coração do seu sistema. Aqui ficam as regras de negócio puras.

### Contém:
- Entities (Client, Account, Transaction…)
- Interfaces (contratos dos repositórios)
- Regras de negócio
- Value Objects (se houver)
- Enums

### NÃO PODE conter:

- SQL
- SQLite
- Web
- Console
- Frameworks

### Exemplos:
```c#
// Domain/Entities/Client.cs
public class Client
{
    public string Cpf { get; set; }
    public string Password { get; set; }
}
```

```c#
// Domain/Interfaces/IClientRepository.cs
public interface IClientRepository
{
    void Create(Client client);
    List<Client> Get();
    Client? GetByCpf(string cpf);
    void Update(Client client);
    void Delete(string cpf);
}
```

#### OBS: Aqui só deve ter regra do negócio, não deve ter banco, nem SQL.

## 2. Application (Camada de Aplicação)

É a camada que usa o Domínio para executar as ações do sistema (casos de uso).

Ela faz a ponte entre:

`[Interface do usuário] → [Domínio] → [Infraestrutura]`

#### Contém:
- Services (ClientService, AccountService…)
- DTOs (se quiser)
- Casos de uso (Use Cases)

Exemplo:
```c#
// Application/Services/ClientService.cs
public class ClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public void CreateClient(string cpf, string password)
    {
        var client = new Client
        {
            Cpf = cpf,
            Password = password
        };

        _repository.Create(client);
    }

    public List<Client> GetClients()
    {
        return _repository.Get();
    }
}
```


#### Obs: Ele não sabe se é SQLite, SQL Server ou Mongo... only interface: IClientRepository.

## 3. Infrastructure (Camada de Infraestrutura)

Essa é a camada que faz acontecer no mundo real. Aqui entra essencialmente:
- SQLite
- SQL Server
- APIs externas
- Arquivos
- Repositórios reais

Ela implementa as interfaces do domínio.

Exemplo:
```c#
// Infrastructure/Repositories/ClientRepository.cs
public class ClientRepository : IClientRepository
{
    private readonly DbService _db;

    public ClientRepository(DbService db)
    {
        _db = db;
    }

    public void Create(Client client)
    {
        using var conn = _db.Connection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "INSERT INTO CLIENTS (CPF, PASSWORD) VALUES (@cpf, @password)";
        cmd.Parameters.AddWithValue("@cpf", client.Cpf);
        cmd.Parameters.AddWithValue("@password", client.Password);
        cmd.ExecuteNonQuery();
    }

    public List<Client> Get()
    {
        var list = new List<Client>();

        using var conn = _db.Connection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT CPF, PASSWORD FROM CLIENTS";
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Client
            {
                Cpf = reader.GetString(0),
                Password = reader.GetString(1)
            });
        }

        return list;
    }

    public Client? GetByCpf(string cpf)
    {
        using var conn = _db.Connection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT CPF, PASSWORD FROM CLIENTS WHERE CPF = @cpf";
        cmd.Parameters.AddWithValue("@cpf", cpf);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Client
            {
                Cpf = reader.GetString(0),
                Password = reader.GetString(1)
            };
        }

        return null;
    }

    public void Update(Client client)
    {
        using var conn = _db.Connection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "UPDATE CLIENTS SET PASSWORD = @pass WHERE CPF = @cpf";
        cmd.Parameters.AddWithValue("@cpf", client.Cpf);
        cmd.Parameters.AddWithValue("@pass", client.Password);

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
```
#### Obs: aqui fica tudo relacionado ao banco de dados, DBservice, migrations, sqlite, tudo.


## 4. Presentation (Camada de apresentação)

É onde está o ponto de entrada do seu sistema, essencialmente pode ser qualquer coisa como por exemplo:

- Console app
- API (ASP.NET)
- MVC (ASP.NET)
- Web (Angular, React, Nuxt e etc)
- Mobile (Flutter, React-native, Maui e etc)

Essa camada só recebe o input e chama a camada Application, segue abaixo o exemplo:
```c#
// Presentation/Program.cs
var builder = new ServiceCollection();

builder.AddSingleton<DbService>();
builder.AddScoped<IClientRepository, ClientRepository>();
builder.AddScoped<ClientService>();

var provider = builder.BuildServiceProvider();

var service = provider.GetRequiredService<ClientService>();

service.CreateClient("12345678900", "1234");

var clients = service.GetClients();

foreach (var client in clients)
{
    Console.WriteLine(client.Cpf);
}
```
Abaixo vou deixar uma tree de arquivos afins de exemplo numa configuração de Clean Arch com 4 projetos:
````bash
VirtualBank/
├── VirtualBank.sln
├── docs/
├── tests/
├── docker/
└── src/
    ├── VirtualBank.Domain
    │   ├── Entities/
    │   │   ├── Client.cs
    │   │   └── Account.cs
    │   │
    │   ├── Interfaces/
    │   │   ├── IClientRepository.cs
    │   │   └── IAccountRepository.cs
    │   │
    │   └── Enums/
    │       └── AccountType.cs
    │ 
    ├── VirtualBank.Application
    │   ├── Services/
    │   │   ├── ClientService.cs
    │   │   └── AccountService.cs
    │   │
    │   └── DTOs/
    │       └── CreateClientDTO.cs
    │ 
    ├── VirtualBank.Infrastructure
    │   ├── Database/
    │   │   └── data.db
    │   │
    │   ├── Migrations/
    │   │   └── 01122025_create_tables.sql
    │   │
    │   ├── Repositories/
    │   │   ├── ClientRepository.cs
    │   │   └── AccountRepository.cs
    │   │
    │   └── Services/
    │       └── DbService.cs
    │ 
    └── VirtualBank.Presentation
        ├── Program.cs
        └── UI
            └── Menu.cs
````
#### E você não é obrigado a ter 4 projetos (.csproj), segue abaixo também uma estrutura de clean arch em um único projeto:
```bash
VirtualBank/
├── VirtualBank.sln
├── docs/
├── tests/
├── docker/
└── src/
    ├── Domain/
    ├── Application/
    ├── Infrastructure/
    └── Presentation/
```