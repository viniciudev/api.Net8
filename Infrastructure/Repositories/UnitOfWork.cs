namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextClass _dbContext;

        public IClientRepository Client { get; }

        public UnitOfWork(
            DbContextClass dbContext,
            IClientRepository clientRepository
            ) 
        {
            _dbContext = dbContext;
            Client = clientRepository;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        IClientRepository Client { get; }
        int Save();
    }
}
