using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IDBRepository : IDisposable
    {
        /// <summary>
        /// Save all changes made to the database
        /// </summary>
        /// <returns></returns>
        Task<bool> Complete();

        /// <summary>
        /// Add an item to the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Add list of items to the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task AddRangeAsync<T>(IEnumerable<T> entity) where T : class;

        /// <summary>
        /// Remove an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Update a list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UpdateRange<T>(ICollection<T> entity) where T : class;

        /// <summary>
        /// Remove an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Remove<T>(T entity) where T : class;

        /// <summary>
        /// Remove a list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RemoveRange<T>(ICollection<T> entity) where T : class;

        /// <summary>
        /// Remove a list of items from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RemoveRange<T>(IQueryable<T> entity) where T : class;

        /// <summary>
        /// Find an item from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<T> FindAsync<T>(Expression<Func<T, bool>> search) where T : class;

        /// <summary>
        /// Find many items where condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        IQueryable<T> FindMany<T>(Expression<Func<T, bool>> search) where T : class;
    }

}
