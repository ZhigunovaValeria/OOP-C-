using System.Globalization;
using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class GoodDaoFileRepository : BaseTextFileRepository<GoodDao>
{
    public GoodDaoFileRepository(string filePath = "goods.txt") : base(filePath) { }

    protected override GoodDao Parse(string[] parts)
    {
        if (parts.Length != 3)
            throw new InvalidDataException("Ожидалось три TXT-поля.");
        return new GoodDao
        {
            Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
            Name = parts[1],
            Code = parts[2]
        };
    }

    protected override string Format(GoodDao item) =>
        $"{item.Id}{Delimiter}{TextField(item.Name)}{Delimiter}{TextField(item.Code)}";
}
