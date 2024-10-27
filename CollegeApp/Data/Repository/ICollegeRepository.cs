using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
    public interface ICollegeRepository<T>
    {
        Task<T> Create(T dbRecord);

        Task<bool> Delete(T dbRecord);

        Task<List<T>> GetAll();

        Task<T> GetById(Expression<Func<T, bool>> filter, bool useNoTracking = false);

        Task<T> GetByName(Expression<Func<T, bool>> filter);

        Task<T> Update(T dbRecord);
    }
}