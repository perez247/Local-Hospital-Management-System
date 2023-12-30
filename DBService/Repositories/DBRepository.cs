using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DBService.Repositories
{
    public class DBRepository : IDBRepository
    {
        private readonly AppDBContext _context;

        public DBRepository(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add an item to the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// Add list of items to the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task AddRangeAsync<T>(IEnumerable<T> entity) where T : class
        {
            await _context.Set<T>().AddRangeAsync(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// Remove an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Update a list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UpdateRange<T>(ICollection<T> entity) where T : class
        {
            _context.Set<T>().UpdateRange(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Remove an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Find an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> search) where T : class
        {
            return await _context.Set<T>().FirstOrDefaultAsync(search);
        }

        /// <summary>
        /// Find many items where condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IQueryable<T> FindMany<T>(Expression<Func<T, bool>> search) where T : class
        {
            return _context.Set<T>().Where(search);
        }

        /// <summary>
        /// Remove a list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveRange<T>(ICollection<T> entity) where T : class
        {
            _context.Set<T>().RemoveRange(entity);
        }

        /// <summary>
        /// Remove a list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveRange<T>(IQueryable<T> entity) where T : class
        {
            _context.Set<T>().RemoveRange(entity);
        }

        /// <summary>
        /// Save all changes made to the database
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Complete()
        {
            if (await _context.SaveChangesAsync() <= 0)
                throw new CustomMessageException("It seems we are having issues saving at the moment");

            return true;
        }

        /// <summary>
        /// Overrides the dispose method
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
