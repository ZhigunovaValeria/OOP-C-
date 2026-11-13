using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ShopDaoXmlRepository : BaseXmlFileRepository<ShopDao>
{
    public ShopDaoXmlRepository(string filePath = "shops.xml")
        : base(filePath) { }
}
