using VirtualBank.Domain.Entities;

namespace VirtualBank.Domain.Interfaces;

public interface IAccountRepository : IRepository<Account, string>
{
    
}