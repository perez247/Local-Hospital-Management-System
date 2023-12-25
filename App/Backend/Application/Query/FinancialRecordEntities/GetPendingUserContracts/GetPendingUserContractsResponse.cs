using Application.Responses;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetPendingUserContracts
{
    public class GetPendingUserContractsResponse : AppCostResponse
    {
        public PatientContractResponse? PatientContractObj { get; set; }
        public CompanyContractResponse? CompanyContractObj { get; set; }

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

            data.PatientContractObj = PatientContractResponse.Create(appCost?.PatientContract);
            data.CompanyContractObj = CompanyContractResponse.Create(appCost?.CompanyContract);

            if (data.CompanyContractObj != null)
            {
                data.CompanyContractObj.Company = CompanyResponse.Create(appCost?.CompanyContract.Company);
            }

            if (data.PatientContractObj != null)
            {
                data.PatientContractObj.Patient = PatientResponse.Create(appCost?.PatientContract.Patient);
            }

            return data;
        }
    }
}
