using System;
using System.Collections.Generic;

public class ClientDaoFileRepository : BaseFileRepository<ClientDao>
{
    public ClientDaoFileRepository() : base("clients.txt") { }

    protected override ClientDao ParseLine(string line)
    {
        string[] parts = line.Split(';');
        return new ClientDao(
            int.Parse(parts[0]),
            parts[1],
            parts[2],
            parts[3],
            DateTime.Parse(parts[4])
        );
    }

    protected override string ToLine(ClientDao item)
    {
        return $"{item.Id};{item.FirstName};{item.LastName};{item.MiddleName};{item.BirthDate:yyyy-MM-dd}";
    }

    protected override bool IsSame(ClientDao existing, ClientDao target)
    {
        return existing.Id == target.Id;
    }

    public override void Update(ClientDao item)
    {
        List<ClientDao> allItems = ReadAll();
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].Id == item.Id)
            {
                allItems[i] = item;
                break;
            }
        }
        SaveAll(allItems);
    }
}