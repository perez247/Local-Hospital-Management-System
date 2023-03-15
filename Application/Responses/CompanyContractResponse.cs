using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class CompanyContractResponse
    {
        public virtual AppCostResponse? AppCost { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public CompanyResponse? Company { get; set; }
        public static CompanyContractResponse? Create(CompanyContract companyContract)
        {
            if (companyContract == null)
            {
                return null;
            }

            return new CompanyContractResponse
            {
                AppCost = AppCostResponse.Create(companyContract.AppCost),
                StartDate = companyContract.StartDate,
                Duration = companyContract.Duration,
            };
        }
    }
}
