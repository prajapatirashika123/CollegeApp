using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly CollegeDBContext _dBContext;
        private DbSet<T> _dbSet;

        public CollegeRepository(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;
            _dbSet = _dBContext.Set<T>();
        }

        public async Task<T> Create(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dBContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> Delete(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetByName(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> Update(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            await _dBContext.SaveChangesAsync();
            return dbRecord;
        }
    }
}