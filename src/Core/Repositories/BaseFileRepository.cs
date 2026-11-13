using Core.Interfaces;

namespace Core.Repositories;

/// <summary>Общие CRUD-операции; формат файла определяется дочерним классом.</summary>
public abstract class BaseFileRepository<T> : IRepository<T> where T : class, IEntity
{
    protected string FilePath { get; }

    protected BaseFileRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        FilePath = Path.GetFullPath(filePath);
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
    }

    protected abstract List<T> LoadData();
    protected abstract void SaveData(List<T> items);

    public void Create(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var items = LoadData();
        if (item.Id < 0)
            throw new ArgumentOutOfRangeException(nameof(item), "Id не может быть отрицательным.");
        if (item.Id == 0)
            item.Id = checked(items.Select(x => x.Id).DefaultIfEmpty().Max() + 1);
        if (items.Any(x => x.Id == item.Id))
            throw new InvalidOperationException($"Объект с Id {item.Id} уже существует.");
        items.Add(item);
        SaveData(items);
    }

    public T? Read(int id) => LoadData().FirstOrDefault(x => x.Id == id);

    public IEnumerable<T> ReadAll() => LoadData();

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var items = LoadData();
        int index = items.FindIndex(x => x.Id == item.Id);
        if (index < 0)
            throw new KeyNotFoundException($"Объект с Id {item.Id} не найден.");
        items[index] = item;
        SaveData(items);
    }

    public void Delete(int id)
    {
        var items = LoadData();
        if (items.RemoveAll(x => x.Id == id) > 0)
            SaveData(items);
    }
}
