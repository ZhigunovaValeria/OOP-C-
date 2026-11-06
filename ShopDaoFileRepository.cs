using System.Collections.Generic;

public class ShopDaoFileRepository : BaseFileRepository<ShopDao>
{
    public ShopDaoFileRepository() : base("shops.txt") { }

    protected override ShopDao ParseLine(string line)
    {
        string[] parts = line.Split(';');
        return new ShopDao(int.Parse(parts[0]), parts[1], parts[2]);
    }

    protected override string ToLine(ShopDao item)
    {
        return $"{item.Id};{item.Name};{item.Code}";
    }

    protected override bool IsSame(ShopDao existing, ShopDao target)
    {
        return existing.Id == target.Id;
    }

    public override void Update(ShopDao item)
    {
        List<ShopDao> allItems = ReadAll();
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