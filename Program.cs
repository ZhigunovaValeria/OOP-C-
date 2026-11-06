using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        ShopDaoFileRepository shopRepo = new ShopDaoFileRepository();
        ClientDaoFileRepository clientRepo = new ClientDaoFileRepository();
        GoodDaoFileRepository goodRepo = new GoodDaoFileRepository();

        ShopDao shop1 = new ShopDao(1, "Магазин А", "SHOP001");
        ShopDao shop2 = new ShopDao(2, "Магазин Б", "SHOP002");
        shopRepo.Create(shop1);
        shopRepo.Create(shop2);

        Console.WriteLine("Все магазины:");
        List<ShopDao> shops = shopRepo.ReadAll();
        foreach (ShopDao shop in shops)
        {
            Console.WriteLine($"ID: {shop.Id}, Название: {shop.Name}, Код: {shop.Code}");
        }

        ShopDao shopToUpdate = new ShopDao(1, "Магазин А (обновленный)", "SHOP001");
        shopRepo.Update(shopToUpdate);
        Console.WriteLine("\nПосле обновления:");
        shops = shopRepo.ReadAll();
        foreach (ShopDao shop in shops)
        {
            Console.WriteLine($"ID: {shop.Id}, Название: {shop.Name}, Код: {shop.Code}");
        }

        shopRepo.Delete(shop2);
        Console.WriteLine("\nПосле удаления магазина Б:");
        shops = shopRepo.ReadAll();
        foreach (ShopDao shop in shops)
        {
            Console.WriteLine($"ID: {shop.Id}, Название: {shop.Name}, Код: {shop.Code}");
        }

        ClientDao client1 = new ClientDao(1, "Иван", "Иванов", "Иванович", new DateTime(1990, 5, 15));
        ClientDao client2 = new ClientDao(2, "Мария", "Петрова", "Сергеевна", new DateTime(1985, 10, 20));
        clientRepo.Create(client1);
        clientRepo.Create(client2);

        Console.WriteLine("\nВсе клиенты:");
        List<ClientDao> clients = clientRepo.ReadAll();
        foreach (ClientDao client in clients)
        {
            Console.WriteLine($"ID: {client.Id}, ФИО: {client.LastName} {client.FirstName} {client.MiddleName}, Возраст: {client.Age}");
        }

        GoodDao good1 = new GoodDao(1, "Товар А", "GOOD001");
        GoodDao good2 = new GoodDao(2, "Товар Б", "GOOD002");
        goodRepo.Create(good1);
        goodRepo.Create(good2);

        Console.WriteLine("\nВсе товары:");
        List<GoodDao> goods = goodRepo.ReadAll();
        foreach (GoodDao good in goods)
        {
            Console.WriteLine($"ID: {good.Id}, Название: {good.Name}, Код: {good.Code}");
        }

        Console.ReadLine();
    }
}