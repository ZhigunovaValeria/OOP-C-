using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using Core.Interfaces;

namespace Core.Repositories;

public abstract class BaseJsonFileRepository<T> : BaseFileRepository<T> where T : class, IEntity
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    protected BaseJsonFileRepository(string filePath) : base(filePath) { }

    protected override List<T> LoadData()
    {
        if (!File.Exists(FilePath))
            return [];
        return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(FilePath, Encoding.UTF8), Options)
            ?? throw new InvalidDataException("Ожидался JSON-массив.");
    }

    protected override void SaveData(List<T> items) =>
        File.WriteAllText(FilePath, JsonSerializer.Serialize(items, Options), Encoding.UTF8);
}
