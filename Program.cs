using System;
using System.Collections.Generic;

namespace ShopApp
{
    // Класс: Касса (сервис определения цены в момент продажи)
    public class CashRegister
    {
        private readonly Dictionary<int, decimal> _currentPrices = new Dictionary<int, decimal>
        {
            { 1001, 450.50m },
            { 1002, 45.00m },
            { 1003, 135.00m }
        };

        public decimal GetPrice(Good good)
        {
            if (_currentPrices.TryGetValue(good.Id, out decimal price)) return price;
            throw new KeyNotFoundException($"Цена для товара '{good.Name}' (ID: {good.Id}) не найдена в системе.");
        }

        public void UpdatePrice(int goodId, decimal newPrice)
        {
            if (_currentPrices.ContainsKey(goodId))
            {
                _currentPrices[goodId] = newPrice;
                Console.WriteLine($"[СИСТЕМА] Цена товара ID {goodId} обновлена: {newPrice} руб.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
         
            // 11 и 16: Работа с файлами магазинов
           
            Console.WriteLine("--- Задания 11 и 16: Работа с реестром магазинов ---");
            string shopsFileName = "shops.txt";

            List<Shop> shopsRegistry = DataManager.InitShops();
            Console.WriteLine($"[OK] Инициализировано {shopsRegistry.Count} базовых магазинов.");

            List<Shop> additionalShops = DataManager.CreateNewShops();
            Console.WriteLine($"[OK] Создано {additionalShops.Count} новых магазинов через конструктор.");

            shopsRegistry.AddRange(additionalShops);
            Console.WriteLine($"[OK] Объединенный реестр содержит {shopsRegistry.Count} магазинов.");

            DataManager.WriteShops(shopsRegistry, shopsFileName);
            Console.WriteLine($"[OK] Файл {shopsFileName} успешно обновлен.\n");

            List<Shop> loadedShops = DataManager.ReadShops(shopsFileName);
            DataManager.PrintShops(loadedShops);
            Console.WriteLine();

            Shop shop = loadedShops[0];
            Client clientAdult = new Client(101, "Иван", "Иванов", "Иванович", new DateTime(2000, 2, 1));
            Client clientMinor = new Client(102, "Пётр", "Петров", "Сергеевич", new DateTime(2010, 9, 15));

            Good goodWine = new Good(1001, "Вино красное сухое", "WINE-RED-001");
            Good goodBread = new Good(1002, "Хлеб бородинский", "BREAD-002");

            Console.WriteLine("--- Демонстрация интерфейса IPrimary ---");
            PrintEntityId(shop);
            PrintEntityId(clientAdult);
            PrintEntityId(goodWine);
            Console.WriteLine();
            // 8, 9 и 14: Работа с файлами товаров
            
            Console.WriteLine("--- Задания 8, 9 и 14: Работа с реестром товаров ---");
            string goodsFileName = "goods.txt";

            List<Good> goodsRegistry = DataManager.InitGoods();
            Console.WriteLine($"[OK] Инициализировано {goodsRegistry.Count} базовых товаров.");

            List<Good> additionalGoods = DataManager.CreateNewGoods();
            Console.WriteLine($"[OK] Создано {additionalGoods.Count} новых товаров через конструктор.");

            goodsRegistry.AddRange(additionalGoods);
            Console.WriteLine($"[OK] Объединенный реестр содержит {goodsRegistry.Count} товаров.");

            DataManager.WriteGoods(goodsRegistry, goodsFileName);
            Console.WriteLine($"[OK] Файл {goodsFileName} успешно обновлен.\n");

            List<Good> loadedGoods = DataManager.ReadGoods(goodsFileName);
            DataManager.PrintGoods(loadedGoods);
            Console.WriteLine();

            // 10 и 15: Работа с файлами клиентов
            Console.WriteLine("--- Задания 10 и 15: Работа с реестром клиентов ---");
            string clientsFileName = "clients.txt";

            List<Client> clientsRegistry = DataManager.InitClients();
            Console.WriteLine($"[OK] Инициализировано {clientsRegistry.Count} базовых клиентов.");

            List<Client> additionalClients = DataManager.CreateNewClients();
            Console.WriteLine($"[OK] Создано {additionalClients.Count} новых клиентов через конструктор.");

            clientsRegistry.AddRange(additionalClients);
            Console.WriteLine($"[OK] Объединенный реестр содержит {clientsRegistry.Count} клиентов.");

            DataManager.WriteClients(clientsRegistry, clientsFileName);
            Console.WriteLine($"[OK] Файл {clientsFileName} успешно обновлен.\n");

            List<Client> loadedClients = DataManager.ReadClients(clientsFileName);
            DataManager.PrintClients(loadedClients);
            Console.WriteLine();

            CashRegister register = new CashRegister();

            Console.WriteLine("--- Тестирование системы продаж ---\n");
            ProcessSale(clientAdult, goodBread, register);
            ProcessSale(clientMinor, goodWine, register);
            ProcessSale(clientAdult, goodWine, register);

            Console.WriteLine("--- Наступил вечер, включена акция ---");
            register.UpdatePrice(1001, 399.90m);
            ProcessSale(clientAdult, goodWine, register);
        }

        // общие методы сценария
        static void PrintEntityId(IPrimary entity)
        {
            Console.WriteLine($"Сущность типа [{entity.GetType().Name,-6}] имеет уникальный ID: {entity.Id}");
        }

        static void ProcessSale(Client client, Good good, CashRegister register)
        {
            Console.WriteLine($"Попытка продажи: {good.Name}");
            bool isAgeRestricted = good.Name.Contains("Вино", StringComparison.OrdinalIgnoreCase) ||
                                   good.Name.Contains("Сигареты", StringComparison.OrdinalIgnoreCase);

            if (isAgeRestricted && !client.IsAdult)
            {
                Console.WriteLine($"ОТКАЗ: Покупатель {client.GetFullName()} ({client.Age} лет). Продажа запрещена законом.");
                return;
            }

            decimal currentPrice = register.GetPrice(good);
            Console.WriteLine($"УСПЕХ: Товар '{good.Name}' продан покупателю {client.GetFullName()}.");
            Console.WriteLine($"   Списано с карты: {currentPrice} руб.\n");
        }
    }
}