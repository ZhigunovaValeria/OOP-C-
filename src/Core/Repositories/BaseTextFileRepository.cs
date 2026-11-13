using System.Text;
using Core.Interfaces;

namespace Core.Repositories;

/// <summary>TXT-формат исходного задания: поля разделены символом |.</summary>
public abstract class BaseTextFileRepository<T> : BaseFileRepository<T> where T : class, IEntity
{
    protected const char Delimiter = '|';

    protected BaseTextFileRepository(string filePath) : base(filePath) { }

    protected abstract T Parse(string[] parts);
    protected abstract string Format(T item);

    protected override List<T> LoadData()
    {
        if (!File.Exists(FilePath))
            return [];
        return File.ReadLines(FilePath, Encoding.UTF8)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => Parse(line.Split(Delimiter)))
            .ToList();
    }

    protected override void SaveData(List<T> items) =>
        File.WriteAllLines(FilePath, items.Select(Format), Encoding.UTF8);

    protected static string TextField(string? value)
    {
        value ??= "";
        if (value.IndexOfAny(['|', '\r', '\n']) >= 0)
            throw new ArgumentException("TXT-поле не должно содержать | или перенос строки.");
        return value;
    }
}
