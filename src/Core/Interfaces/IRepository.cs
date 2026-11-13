namespace Core.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    void Create(T item);
    T? Read(int id);
    IEnumerable<T> ReadAll();
    void Update(T item);
    void Delete(int id);
}
