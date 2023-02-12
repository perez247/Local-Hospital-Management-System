using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetUserList
{
    public class GetUserListFilter
    {
        public string? Name { get; set; }
        public bool? Active { get; set; }

        [VerifyGuidAnnotation]
        public string? StaffId { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public string? UserType { get; set; }
        public bool? IsCompany { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientCompanyId { get; set; }
    }
}
