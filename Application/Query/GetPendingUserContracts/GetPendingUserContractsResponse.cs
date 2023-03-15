using Application.Responses;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetPendingUserContracts
{
    public class GetPendingUserContractsResponse : AppCostResponse
    {
        public PatientContractResponse? PatientContract { get; set; }
        public CompanyContractResponse? CompanyContract { get; set; }

        public static GetPendingUserContractsResponse? Create(AppCost appCost)
        {
            if (appCost == null)
            {
                return null;
            }



            var parent = AppCostResponse.Create(appCost);
            var serializedParent = JsonConvert.SerializeObject(parent);
            GetPendingUserContractsResponse data = JsonConvert.DeserializeObject<GetPendingUserContractsResponse>(serializedParent);

            if (data == null)
            {
                return null;
            }

            data.PatientContract = PatientContractResponse.Create(appCost?.PatientContract);
            data.CompanyContract = CompanyContractResponse.Create(appCost?.CompanyContract);
            
            if (data.CompanyContract != null)
            {
                data.CompanyContract.Company = CompanyResponse.Create(appCost?.CompanyContract.Company);
            }

            if (data.PatientContract != null)
            {
                data.PatientContract.Patient = PatientResponse.Create(appCost?.PatientContract.Patient);
            }

            return data;
        }
    }
}
