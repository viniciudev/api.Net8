using Core.Models;

namespace Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(DbContextClass dbContext) : base(dbContext)
        {
        }
    }
    public interface IClientRepository : IGenericRepository<Client>
    {
    }
}
