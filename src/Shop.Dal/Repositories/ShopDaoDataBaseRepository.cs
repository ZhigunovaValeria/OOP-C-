using Core.Repositories;
using Shop.Dal.Database;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ShopDaoDataBaseRepository : BaseDataBaseRepository<ShopDao>
{
    public ShopDaoDataBaseRepository(ShopEntityContext context)
        : base(context, context.Shops) { }
}
