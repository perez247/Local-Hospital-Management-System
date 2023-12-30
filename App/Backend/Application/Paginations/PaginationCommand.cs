using Application.Annotations;
using Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Paginations
{
    public class PaginationCommand : TokenCredentials
    {
        /// <summary>
        /// Number of the page
        /// </summary>
        /// <value></value>
        [NumberRangeAnnotation(1, 20000, 1)]
        public int PageNumber { get; set; }

        /// <summary>
        /// Size of the page
        /// </summary>
        /// <value></value>
        [NumberRangeAnnotation(1, 500, 10)]
        public int PageSize { get; set; }
    }
}
