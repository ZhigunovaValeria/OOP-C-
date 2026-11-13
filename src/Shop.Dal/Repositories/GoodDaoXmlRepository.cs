using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class GoodDaoXmlRepository : BaseXmlFileRepository<GoodDao>
{
    public GoodDaoXmlRepository(string filePath = "goods.xml")
        : base(filePath) { }
}
