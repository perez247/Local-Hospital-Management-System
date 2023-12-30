using Application.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Paginations
{
    /// <summary>
    /// Pagination helper class
    /// </summary>
    public static class PaginationQueryHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> AddPagination<T>(this IQueryable<T> query, PaginationCommand filter)
        {
            return query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            // return query;
        }

        /// <summary>
        /// Generate initial pagiantion value
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<PaginationDto<T>> GenerateEntity<T>(this IQueryable<T> query, PaginationCommand filter)
        {
            var data = new PaginationDto<T>();

            data.totalItems = await query.CountAsync();

            data.Results = await query.AddPagination<T>(filter).ToListAsync();

            return data;
        }

        /// <summary>
        /// Generate initial pagiantion value
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="result"></param>
        /// <param name="fromApplication"></param>
        /// <param name="resultOwner"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static PaginationResponse<T> GenerateResponse<T, K>(this PaginationCommand filter, T result, PaginationDto<K> fromApplication, UserResponse resultOwner = null)
        {
            var data = new PaginationResponse<T>();

            data.totalItems = fromApplication.totalItems;

            data.Result = result;

            data.PageNumber = filter.PageNumber;

            data.PageSize = filter.PageSize;

            // data.ResultOwner = resultOwner == null ? null : UserResponse.Create(resultOwner);

            return data;
        }
    }
}
