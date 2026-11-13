using System.Text;
using Core.Interfaces;
using Shop.Bll.Entities;
using Shop.Dal.Entities;
using Shop.Dal.Repositories;

namespace Shop.ConsoleApp;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        string dataPath = args.Length == 0
            ? Path.Combine(AppContext.BaseDirectory, "data")
            : Path.GetFullPath(args[0]);
        Directory.CreateDirectory(dataPath);
        Console.WriteLine("Задание 3: справочники магазина");
        Console.WriteLine($"Данные: {dataPath}");
        SeedFiles(dataPath);
        DemonstrateBusinessRules();

    }

    private static void SeedFiles(string path)
    {
        foreach (string format in new[] { "txt", "xml", "json" })
        {
            IRepository<ShopDao> shops = format switch
            {
                "txt" => new ShopDaoFileRepository(Path.Combine(path, "shops.txt")),
                "xml" => new ShopDaoXmlRepository(Path.Combine(path, "shops.xml")),
                _ => new ShopDaoJsonRepository(Path.Combine(path, "shops.json"))
            };
            IRepository<ClientDao> clients = format switch
            {
                "txt" => new ClientDaoFileRepository(Path.Combine(path, "clients.txt")),
                "xml" => new ClientDaoXmlRepository(Path.Combine(path, "clients.xml")),
                _ => new ClientDaoJsonRepository(Path.Combine(path, "clients.json"))
            };
            IRepository<GoodDao> goods = format switch
            {
                "txt" => new GoodDaoFileRepository(Path.Combine(path, "goods.txt")),
                "xml" => new GoodDaoXmlRepository(Path.Combine(path, "goods.xml")),
                _ => new GoodDaoJsonRepository(Path.Combine(path, "goods.json"))
            };
            Seed(shops, CreateShops());
            Seed(clients, CreateClients());
            Seed(goods, CreateGoods());
            Console.WriteLine($"{format.ToUpperInvariant()}: магазинов {shops.ReadAll().Count()}, " +
                $"клиентов {clients.ReadAll().Count()}, товаров {goods.ReadAll().Count()}.");
        }
    }

    // Заполняются только отсутствующие Id: повторный запуск не дублирует записи.
    private static void Seed<T>(IRepository<T> repository, IEnumerable<T> items)
        where T : class, IEntity
    {
        foreach (var item in items)
            if (repository.Read(item.Id) is null)
                repository.Create(item);
    }

    private static ShopDao[] CreateShops() =>
    [
        new() { Id = 1, Name = "Пятёрочка", Code = "SHOP-001" },
        new() { Id = 2, Name = "Магнит", Code = "SHOP-002" },
        new() { Id = 3, Name = "Перекрёсток", Code = "SHOP-003" }
    ];

    private static ClientDao[] CreateClients() =>
    [
        new() { Id = 1, FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", BirthDate = new(1990, 5, 15) },
        new() { Id = 2, FirstName = "Анна", LastName = "Петрова", MiddleName = null, BirthDate = new(1995, 10, 20) },
        new() { Id = 3, FirstName = "Сергей", LastName = "Сидоров", MiddleName = "Александрович", BirthDate = new(1985, 3, 8) },
        new() { Id = 4, FirstName = "Ольга", LastName = "Васильева", MiddleName = "Николаевна", BirthDate = new(1990, 7, 12) },
        new() { Id = 5, FirstName = "Елена", LastName = "Смирнова", MiddleName = "Дмитриевна", BirthDate = new(1980, 1, 10) }
    ];

    private static IEnumerable<GoodDao> CreateGoods()
    {
        string[] names =
        [
            "Хлеб бородинский", "Молоко 3.2%", "Сыр Российский", "Колбаса Докторская",
            "Вода минеральная", "Сок апельсиновый", "Шоколад молочный", "Чай чёрный",
            "Кофе зерновой", "Сахар-песок", "Соль поваренная", "Макароны спагетти",
            "Рис шлифованный", "Масло подсолнечное", "Яйца куриные С1", "Филе куриное",
            "Минтай свежемороженый", "Огурцы свежие", "Яблоки Голден", "Бананы",
            "Масло сливочное 82.5%", "Йогурт натуральный", "Сметана 20%", "Творог 9%",
            "Кефир 1%", "Сыр Моцарелла", "Пельмени домашние", "Сосиски молочные",
            "Чай зелёный", "Печенье овсяное"
        ];
        for (int i = 0; i < names.Length; i++)
            yield return new GoodDao { Id = i + 1, Name = names[i], Code = $"ART-{i + 1:D4}" };
    }

    private static void DemonstrateBusinessRules()
    {
        var shop = new ShopBlo(1, "Пятёрочка", "SHOP-001");
        var good = new GoodBlo(1, "Хлеб бородинский", "ART-0001");
        var client = new ClientBlo(1, "Иван", "Иванов", "Иванович");
        Console.WriteLine($"BLL: {shop.Name}, {good.Name}, дата рождения по умолчанию {client.Birthday:dd.MM.yyyy}.");
        try { shop.Name = ""; }
        catch (ArgumentException) { Console.WriteLine("BLL: пустое название магазина отклонено."); }
    }

}
