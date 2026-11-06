using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace ShopApp
{
    public static class DataManager
    {
        // методы для shop
        public static List<Shop> InitShops() => new List<Shop>
        {
            new Shop(1, "Продукты у дома", "SHOP-001"),
            new Shop(2, "Магнит", "SHOP-002"),
            new Shop(3, "Пятёрочка", "SHOP-003")
        };

        public static List<Shop> CreateNewShops() => new List<Shop>
        {
            new Shop(4, "Лента", "SHOP-004"),
            new Shop(5, "Ашан", "SHOP-005")
        };

        public static void WriteShops(List<Shop> shops, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine("ID;Название;Код");
            foreach (var shop in shops) writer.WriteLine($"{shop.Id};{shop.Name};{shop.Code}");
        }

        public static List<Shop> ReadShops(string filePath)
        {
            var shops = new List<Shop>();
            if (!File.Exists(filePath)) return shops;
            using StreamReader reader = new StreamReader(filePath);
            reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(';');
                if (parts.Length == 3) shops.Add(new Shop(int.Parse(parts[0]), parts[1], parts[2]));
            }
            return shops;
        }

        public static void PrintShops(List<Shop> shops)
        {
            Console.WriteLine("--- Реестр магазинов (обновленный, из файла shops.txt) ---");
            Console.WriteLine($"{"ID",-5} | {"Название магазина",-25} | {"Код",-10}");
            Console.WriteLine(new string('-', 50));
            foreach (var shop in shops) Console.WriteLine($"{shop.Id,-5} | {shop.Name,-25} | {shop.Code,-10}");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Всего магазинов: {shops.Count}");
        }
        // методы для товаров good
        public static List<Good> InitGoods()
        {
            var goods = new List<Good>();
            string[] baseNames = { "Хлеб бородинский", "Молоко 3.2%", "Сыр Российский", "Колбаса Докторская", "Вода минеральная", "Сок апельсиновый", "Шоколад молочный", "Чай черный", "Кофе зерновой", "Сахар-песок", "Соль поваренная", "Макароны спагетти", "Рис шлифованный", "Масло подсолнечное", "Яйца куриные С1", "Филе куриное", "Минтай свежемороженый", "Огурцы свежие", "Яблоки Голден", "Бананы" };
            for (int i = 0; i < 20; i++) goods.Add(new Good(2000 + i, baseNames[i], $"ART-{2000 + i:D4}"));
            return goods;
        }

        public static List<Good> CreateNewGoods() => new List<Good>
        {
            new Good(2020, "Масло сливочное 82.5%", "ART-2020"), new Good(2021, "Йогурт натуральный", "ART-2021"),
            new Good(2022, "Сметана 20%", "ART-2022"), new Good(2023, "Творог 9%", "ART-2023"),
            new Good(2024, "Кефир 1%", "ART-2024"), new Good(2025, "Сыр Моцарелла", "ART-2025"),
            new Good(2026, "Пельмени домашние", "ART-2026"), new Good(2027, "Сосиски молочные", "ART-2027"),
            new Good(2028, "Чай зеленый", "ART-2028"), new Good(2029, "Печенье овсяное", "ART-2029")
        };

        public static void WriteGoods(List<Good> goods, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine("ID;Название;Код");
            foreach (var good in goods) writer.WriteLine($"{good.Id};{good.Name};{good.Code}");
        }

        public static List<Good> ReadGoods(string filePath)
        {
            var goods = new List<Good>();
            if (!File.Exists(filePath)) return goods;
            using StreamReader reader = new StreamReader(filePath);
            reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(';');
                if (parts.Length == 3) goods.Add(new Good(int.Parse(parts[0]), parts[1], parts[2]));
            }
            return goods;
        }

        public static void PrintGoods(List<Good> goods)
        {
            Console.WriteLine("--- Реестр товаров (обновленный, из файла goods.txt) ---");
            Console.WriteLine($"{"ID",-5} | {"Название товара",-30} | {"Код",-10}");
            Console.WriteLine(new string('-', 55));
            foreach (var good in goods) Console.WriteLine($"{good.Id,-5} | {good.Name,-30} | {good.Code,-10}");
            Console.WriteLine(new string('-', 55));
            Console.WriteLine($"Всего позиций: {goods.Count}");
        }

        //методы для Client
        public static List<Client> InitClients() => new List<Client>
        {
            new Client(101, "Иван", "Иванов", "Иванович", new DateTime(2000, 2, 1)),
            new Client(102, "Пётр", "Петров", "Сергеевич", new DateTime(2010, 9, 15)),
            new Client(103, "Анна", "Сидорова", "Павловна", new DateTime(1995, 5, 20)),
            new Client(104, "Дмитрий", "Кузнецов", "Александрович", new DateTime(2008, 11, 30)),
            new Client(105, "Елена", "Смирнова", "Дмитриевна", new DateTime(1980, 1, 10))
        };

        public static List<Client> CreateNewClients() => new List<Client>
        {
            new Client(106, "Ольга", "Васильева", "Николаевна", new DateTime(1990, 7, 12)),
            new Client(107, "Максим", "Новиков", "Андреевич", new DateTime(2012, 3, 5)),
            new Client(108, "Светлана", "Морозова", "Игоревна", new DateTime(1985, 11, 25))
        };

        public static void WriteClients(List<Client> clients, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine("ID;Имя;Фамилия;Отчество;ДатаРождения");
            foreach (var client in clients) writer.WriteLine($"{client.Id};{client.FirstName};{client.LastName};{client.MiddleName};{client.BirthDate:dd.MM.yyyy}");
        }

        public static List<Client> ReadClients(string filePath)
        {
            var clients = new List<Client>();
            if (!File.Exists(filePath)) return clients;
            using StreamReader reader = new StreamReader(filePath);
            reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(';');
                if (parts.Length == 5)
                {
                    DateTime birthDate = DateTime.ParseExact(parts[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    clients.Add(new Client(int.Parse(parts[0]), parts[1], parts[2], parts[3], birthDate));
                }
            }
            return clients;
        }

        public static void PrintClients(List<Client> clients)
        {
            Console.WriteLine("--- Реестр клиентов (обновленный, из файла clients.txt) ---");
            Console.WriteLine($"{"ID",-5} | {"ФИО",-35} | {"Дата рождения",-15} | {"Возраст",-8} | {"Совершеннолетний",-10}");
            Console.WriteLine(new string('-', 85));
            foreach (var client in clients)
            {
                string adultStatus = client.IsAdult ? "Да" : "Нет";
                Console.WriteLine($"{client.Id,-5} | {client.GetFullName(),-35} | {client.BirthDate:dd.MM.yyyy,-15} | {client.Age,-8} | {adultStatus,-10}");
            }
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"Всего клиентов: {clients.Count}");
        }
    }
}