using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class CompanyResponse
    {
        public string? Description { get; set; }
        public string? UniqueId { get; set; }
        public string? OtherId { get; set; }
        public string? UserId { get; set; }
        public CompanyContractResponse? CompanyContract { get; set; }
        public BaseResponse? Base { get; set; }
        public string? Name { get; set; }
        public UserOnlyResponse? User { get; set; }

        public static CompanyResponse? Create(Company Company)
        {
            if (Company == null) { return null; }

            var companyResponse = new CompanyResponse();

            companyResponse.Description = Company.Description;
            companyResponse.OtherId = Company.OtherId;
            companyResponse.UniqueId = Company.UniqueId;
            companyResponse.CompanyContract = Company.CompanyContracts != null ? CompanyContractResponse.Create(Company.CompanyContracts.FirstOrDefault()) : null;
            companyResponse.Name = Company?.AppUser?.FirstName;
            companyResponse.UserId = Company?.AppUser?.Id.ToString();
            companyResponse.Base = BaseResponse.Create(Company);
            companyResponse.User = UserOnlyResponse.Create(Company.AppUser);

            return companyResponse;
        }
    }
}
