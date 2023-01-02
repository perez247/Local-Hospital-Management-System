using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class BaseResponse
    {
        /// <summary>
        /// Payment status
        /// </summary>
        /// <value></value>
        public string? Id { get; set; }

        /// <summary>
        /// Date object was created
        /// </summary>
        /// <value></value>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Date modified
        /// </summary>
        /// <value></value>
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// Create a base oject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static BaseResponse Create(object obj)
        {
            var data = new BaseResponse();

            // throw new Exception((obj.GetType().GetProperty("CurrentStaffApprovalStatus") != null).ToString());
            if (obj.GetType().GetProperty("Id") != null)
            {
                data.Id = obj.GetType().GetProperty("Id").GetValue(obj, null).ToString();
            }

            if (obj.GetType().GetProperty("DateCreated") != null)
            {
                data.DateCreated = obj.GetType().GetProperty("DateCreated").GetValue(obj, null) as DateTime?;
            }

            if (obj.GetType().GetProperty("DateModified") != null)
            {
                data.DateModified = obj.GetType().GetProperty("DateModified").GetValue(obj, null) as DateTime?;
            }

            return data;
        }
    }
}
