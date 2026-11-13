using System.Text.Json;
using Core.Interfaces;
using Shop.Bll.Entities;
using Shop.Dal.Entities;
using Shop.Dal.Repositories;
using Shop.Dal.Database;

namespace Shop.Tests;

// Проверки запускаются как обычное консольное приложение, без стороннего тестового фреймворка.
internal static class Program
{
    private static int _checks;

    private static int Main()
    {
        string root = Path.Combine(Path.GetTempPath(), "shop-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            CheckBusinessRules();
            foreach (string format in new[] { "txt", "xml", "json" })
            {
                string path = Path.Combine(root, format);
                Directory.CreateDirectory(path);
                CheckRepository<ShopDao>(() => Shops(format, path),
                    () => new() { Name = "Пятёрочка", Code = "S-001" },
                    item => item.Name = "Магнит");
                CheckRepository<GoodDao>(() => Goods(format, path),
                    () => new() { Name = "Хлеб бородинский", Code = "G-001" },
                    item => item.Code = "G-002");
                CheckRepository<ClientDao>(() => Clients(format, path),
                    () => new() { FirstName = "Анна", LastName = "Петрова", MiddleName = null, BirthDate = new(1995, 10, 20) },
                    item => { item.MiddleName = "Ивановна"; item.BirthDate = new(1988, 3, 21); });
            }
            string databasePath = Path.Combine(root, "shop.db");
            using (var context = new ShopEntityContext(databasePath))
            {
                CheckRepository<ShopDao>(() => new ShopDaoDataBaseRepository(context),
                    () => new() { Name = "Перекрёсток", Code = "S-001" },
                    item => item.Name = "ВкусВилл");
                CheckRepository<GoodDao>(() => new GoodDaoDataBaseRepository(context),
                    () => new() { Name = "Молоко", Code = "G-001" },
                    item => item.Code = "G-002");
                CheckRepository<ClientDao>(() => new ClientDaoDataBaseRepository(context),
                    () => new() { FirstName = "Иван", LastName = "Иванов", BirthDate = new(1990, 5, 15) },
                    item => item.MiddleName = "Сергеевич");
                new ShopDaoDataBaseRepository(context).Create(
                    new ShopDao { Id = 99, Name = "Магнит", Code = "PERSIST-99" });
            }
            using (var reopened = new ShopEntityContext(databasePath))
            {
                Check(new ShopDaoDataBaseRepository(reopened).Read(99)?.Code == "PERSIST-99",
                    "SQLite сохраняет данные между контекстами");
            }
            Console.WriteLine($"Успешно: {_checks} проверок.");
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
        finally
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            // Удаляется только созданная этой проверкой временная папка с уникальным именем.
            Directory.Delete(root, true);
        }
    }

    private static void CheckBusinessRules()
    {
        var shop = new ShopBlo(12, "Магнит", "S-001");
        var good = new GoodBlo(34, "Рис", "G-001");
        var client = new ClientBlo(56, "Иван", "Иванов");
        Check(shop.Id == 12 && good.Id == 34 && client.Id == 56, "Id задаётся конструктором");
        Check(typeof(ShopBlo).GetProperty("Id")!.SetMethod!.IsPrivate, "ShopBlo.Id закрыт на запись");
        Check(typeof(GoodBlo).GetProperty("Id")!.SetMethod!.IsPrivate, "GoodBlo.Id закрыт на запись");
        Check(typeof(ClientBlo).GetProperty("Id")!.SetMethod!.IsPrivate, "ClientBlo.Id закрыт на запись");
        Check(client.Birthday == new DateTime(1900, 1, 1), "Birthday по умолчанию");
        Expect<ArgumentException>(() => shop.Name = "", "пустое название магазина");
        Expect<ArgumentException>(() => shop.Code = " ", "пустой код магазина");
        Expect<ArgumentException>(() => good.Name = "", "пустое название товара");
        Expect<ArgumentException>(() => good.Code = null!, "пустой код товара");
        Expect<ArgumentException>(() => client.FirstName = "", "пустое имя клиента");
        Expect<ArgumentException>(() => client.LastName = " ", "пустая фамилия клиента");
        Expect<ArgumentException>(() => client.Birthday = default, "пустая дата рождения");
        Expect<ArgumentException>(() => new ShopBlo(1, "", "S"), "валидация в конструкторе");
        client.Patronymic = null;
        client.Birthday = new(2000, 5, 6);
        Check(client.Birthday == new DateTime(2000, 5, 6), "изменение Birthday");
        var birthdayToday = new ClientDao { BirthDate = DateTime.Today.AddYears(-20) };
        Check(birthdayToday.Age == 20, "возраст в день рождения");
        var birthdayTomorrow = new ClientDao { BirthDate = DateTime.Today.AddDays(1).AddYears(-20) };
        Check(birthdayTomorrow.Age == 19, "возраст до дня рождения");
    }

    private static void CheckRepository<T>(Func<IRepository<T>> factory, Func<T> create, Action<T> change)
        where T : class, IEntity
    {
        var repository = factory();
        Check(!repository.ReadAll().Any(), "новое хранилище пустое");
        var item = create();
        repository.Create(item);
        Check(item.Id > 0, "выдан идентификатор");
        string original = JsonSerializer.Serialize(item);
        var loaded = factory().Read(item.Id);
        Check(loaded is not null && JsonSerializer.Serialize(loaded) == original, "данные сохранены и прочитаны");
        Check(factory().ReadAll().Count() == 1, "ReadAll");
        var duplicate = create();
        duplicate.Id = item.Id;
        Expect<InvalidOperationException>(() => repository.Create(duplicate), "повторный Id отклонён");
        change(loaded!);
        string updated = JsonSerializer.Serialize(loaded);
        Check(updated != original, "проверяемое изменение действительно меняет данные");
        factory().Update(loaded!);
        Check(JsonSerializer.Serialize(factory().Read(item.Id)) == updated, "Update сохранён");
        factory().Delete(item.Id);
        Check(factory().Read(item.Id) is null, "Delete сохранён");
        Check(!factory().ReadAll().Any(), "хранилище пустое после удаления");
        Expect<KeyNotFoundException>(() => factory().Update(item), "обновление отсутствующей записи");
        factory().Delete(item.Id);
        Check(factory().Read(99999) is null, "отсутствующий Id возвращает null");
    }

    private static IRepository<ShopDao> Shops(string format, string path) => format switch
    {
        "txt" => new ShopDaoFileRepository(Path.Combine(path, "shops.txt")),
        "xml" => new ShopDaoXmlRepository(Path.Combine(path, "shops.xml")),
        _ => new ShopDaoJsonRepository(Path.Combine(path, "shops.json"))
    };

    private static IRepository<GoodDao> Goods(string format, string path) => format switch
    {
        "txt" => new GoodDaoFileRepository(Path.Combine(path, "goods.txt")),
        "xml" => new GoodDaoXmlRepository(Path.Combine(path, "goods.xml")),
        _ => new GoodDaoJsonRepository(Path.Combine(path, "goods.json"))
    };

    private static IRepository<ClientDao> Clients(string format, string path) => format switch
    {
        "txt" => new ClientDaoFileRepository(Path.Combine(path, "clients.txt")),
        "xml" => new ClientDaoXmlRepository(Path.Combine(path, "clients.xml")),
        _ => new ClientDaoJsonRepository(Path.Combine(path, "clients.json"))
    };

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException("Проверка не пройдена: " + message);
        _checks++;
    }

    private static void Expect<TException>(Action action, string message) where TException : Exception
    {
        try { action(); }
        catch (TException) { _checks++; return; }
        throw new InvalidOperationException("Ожидалось исключение: " + message);
    }

}
