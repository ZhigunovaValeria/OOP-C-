using System.Collections.Generic;

public class GoodDaoFileRepository : BaseFileRepository<GoodDao>
{
    public GoodDaoFileRepository() : base("goods.txt") { }

    protected override GoodDao ParseLine(string line)
    {
        string[] parts = line.Split(';');
        return new GoodDao(int.Parse(parts[0]), parts[1], parts[2]);
    }

    protected override string ToLine(GoodDao item)
    {
        return $"{item.Id};{item.Name};{item.Code}";
    }

    protected override bool IsSame(GoodDao existing, GoodDao target)
    {
        return existing.Id == target.Id;
    }

    public override void Update(GoodDao item)
    {
        List<GoodDao> allItems = ReadAll();
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