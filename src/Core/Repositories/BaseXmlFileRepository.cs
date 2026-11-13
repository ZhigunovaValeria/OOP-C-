using System.Xml.Serialization;
using Core.Interfaces;

namespace Core.Repositories;

public abstract class BaseXmlFileRepository<T> : BaseFileRepository<T> where T : class, IEntity
{
    private static readonly XmlSerializer Serializer = new(typeof(List<T>));

    protected BaseXmlFileRepository(string filePath) : base(filePath) { }

    protected override List<T> LoadData()
    {
        if (!File.Exists(FilePath))
            return [];
        using var stream = File.OpenRead(FilePath);
        return (List<T>?)Serializer.Deserialize(stream)
            ?? throw new InvalidDataException("Ожидался XML-список.");
    }

    protected override void SaveData(List<T> items)
    {
        using var stream = File.Create(FilePath);
        Serializer.Serialize(stream, items);
    }
}
