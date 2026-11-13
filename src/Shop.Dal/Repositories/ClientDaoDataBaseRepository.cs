using Core.Repositories;
using Shop.Dal.Database;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ClientDaoDataBaseRepository : BaseDataBaseRepository<ClientDao>
{
    public ClientDaoDataBaseRepository(ShopEntityContext context)
        : base(context, context.Clients) { }
}
