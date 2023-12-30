using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Paginations
{
    /// <summary>
    /// Dto for pagination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationDto<T>
    {
        /// <summary>
        /// The Page number
        /// </summary>
        /// <value></value>
        public int PageNumber { get; set; }

        /// <summary>
        /// The page size maximum of 20 and minimum of 1
        /// </summary>
        /// <value></value>
        public int PageSize { get; set; }

        /// <summary>
        /// Total items in the database or searched
        /// </summary>
        /// <value></value>
        public int totalItems { get; set; }

        /// <summary>
        /// Entity to send back
        /// </summary>
        /// <value></value>
        public ICollection<T> Results { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PaginationDto()
        {
            Results = new List<T>();
        }
    }
}
