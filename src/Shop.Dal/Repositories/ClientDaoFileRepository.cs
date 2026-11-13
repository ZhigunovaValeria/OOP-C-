using System.Globalization;
using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ClientDaoFileRepository : BaseTextFileRepository<ClientDao>
{
    public ClientDaoFileRepository(string filePath = "clients.txt") : base(filePath) { }

    protected override ClientDao Parse(string[] parts)
    {
        if (parts.Length != 5)
            throw new InvalidDataException("Ожидалось пять TXT-полей клиента.");
        return new ClientDao
        {
            Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
            FirstName = parts[1],
            LastName = parts[2],
            MiddleName = string.IsNullOrEmpty(parts[3]) ? null : parts[3],
            BirthDate = DateTime.ParseExact(parts[4], "yyyy-MM-dd", CultureInfo.InvariantCulture)
        };
    }

    protected override string Format(ClientDao item) =>
        $"{item.Id}{Delimiter}{TextField(item.FirstName)}{Delimiter}{TextField(item.LastName)}" +
        $"{Delimiter}{TextField(item.MiddleName)}{Delimiter}{item.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
}
