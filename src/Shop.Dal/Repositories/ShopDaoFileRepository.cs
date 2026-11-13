using System.Globalization;
using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ShopDaoFileRepository : BaseTextFileRepository<ShopDao>
{
    public ShopDaoFileRepository(string filePath = "shops.txt") : base(filePath) { }

    protected override ShopDao Parse(string[] parts)
    {
        if (parts.Length != 3)
            throw new InvalidDataException("Ожидалось три TXT-поля.");
        return new ShopDao
        {
            Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
            Name = parts[1],
            Code = parts[2]
        };
    }

    protected override string Format(ShopDao item) =>
        $"{item.Id}{Delimiter}{TextField(item.Name)}{Delimiter}{TextField(item.Code)}";
}
