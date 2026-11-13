using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ShopDaoJsonRepository : BaseJsonFileRepository<ShopDao>
{
    public ShopDaoJsonRepository(string filePath = "shops.json")
        : base(filePath) { }
}
