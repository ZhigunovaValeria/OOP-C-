using Core.Repositories;
using Shop.Dal.Database;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class GoodDaoDataBaseRepository : BaseDataBaseRepository<GoodDao>
{
    public GoodDaoDataBaseRepository(ShopEntityContext context)
        : base(context, context.Goods) { }
}
