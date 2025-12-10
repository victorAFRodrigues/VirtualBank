namespace VirtualBank.Domain.Interfaces
{
    public interface IRepository<T, in TKey>
    {
        void Create(T entity);
        List<T> Get();
        T? GetOne(TKey id);
        void Update(T entity);
        void Delete(TKey id);
    }
}