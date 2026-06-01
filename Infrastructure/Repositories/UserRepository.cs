using Core;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmail(string email);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContextClass dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
