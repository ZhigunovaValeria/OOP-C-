using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class GoodDaoJsonRepository : BaseJsonFileRepository<GoodDao>
{
    public GoodDaoJsonRepository(string filePath = "goods.json")
        : base(filePath) { }
}
