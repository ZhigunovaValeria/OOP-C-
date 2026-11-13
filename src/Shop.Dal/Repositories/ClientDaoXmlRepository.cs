using Core.Repositories;
using Shop.Dal.Entities;

namespace Shop.Dal.Repositories;

public class ClientDaoXmlRepository : BaseXmlFileRepository<ClientDao>
{
    public ClientDaoXmlRepository(string filePath = "clients.xml")
        : base(filePath) { }
}
