using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ClientDaoJsonRepository : BaseJsonFileRepository<ClientDao>
{
    public ClientDaoJsonRepository(string filePath = "clients.json")
        : base(filePath) { }
}
