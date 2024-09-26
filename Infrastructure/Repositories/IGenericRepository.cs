namespace Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<(IEnumerable<T> Items, int TotalCount)> GetAll(int pageNumber, int pageSize); // Atualizado para retornar dados paginados
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
