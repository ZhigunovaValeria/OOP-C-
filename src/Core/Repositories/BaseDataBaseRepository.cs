using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

/// <summary>Общий EF Core-репозиторий, не зависящий от сущностей магазина.</summary>
public abstract class BaseDataBaseRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _set;

    protected BaseDataBaseRepository(DbContext context, DbSet<T> set)
    {
        _context = context;
        _set = set;
    }

    public void Create(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Id < 0)
            throw new ArgumentOutOfRangeException(nameof(item));
        if (item.Id != 0 && Read(item.Id) is not null)
            throw new InvalidOperationException($"Объект с Id {item.Id} уже существует.");
        _set.Add(item);
        _context.SaveChanges();
        _context.Entry(item).State = EntityState.Detached;
    }

    public T? Read(int id) => _set.AsNoTracking().FirstOrDefault(x => x.Id == id);

    public IEnumerable<T> ReadAll() => _set.AsNoTracking().OrderBy(x => x.Id).ToList();

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (Read(item.Id) is null)
            throw new KeyNotFoundException($"Объект с Id {item.Id} не найден.");
        DetachTracked(item.Id);
        _set.Update(item);
        _context.SaveChanges();
        _context.Entry(item).State = EntityState.Detached;
    }

    public void Delete(int id)
    {
        var item = Read(id);
        if (item is null)
            return;
        DetachTracked(id);
        _set.Remove(item);
        _context.SaveChanges();
        _context.Entry(item).State = EntityState.Detached;
    }

    private void DetachTracked(int id)
    {
        var tracked = _set.Local.FirstOrDefault(x => x.Id == id);
        if (tracked is not null)
            _context.Entry(tracked).State = EntityState.Detached;
    }
}
