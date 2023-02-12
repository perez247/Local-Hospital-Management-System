using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class PatientResponse
    {
        public CompanyResponse? Company { get; set; }
        public string? Allergies { get; set; }
        public BaseResponse? Base { get; set; }
        public PatientContractResponse? PatientContract { get; set; }
        public UserOnlyResponse? User { get; set; }

        public static PatientResponse? Create(Patient patient)
        {
            if (patient == null) { return null; }

            var patientResponse = new PatientResponse();

            patientResponse.Company = CompanyResponse.Create(patient.Company);

            patientResponse.Allergies = patient.Allergies;
            patientResponse.Base = BaseResponse.Create(patient);
            patientResponse.PatientContract = patient.PatientContracts != null ? PatientContractResponse.Create(patient.PatientContracts.FirstOrDefault()) : null;
            patientResponse.User = UserOnlyResponse.Create(patient.AppUser);

            return patientResponse;
        }
    }
}
