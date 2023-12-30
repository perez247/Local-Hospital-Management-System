using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class PatientVitalResponse
    {
        public StaffResponse? Nurse { get; set; }
        public string? Data { get; set; }
        public BaseResponse? Base { get; set; }

        public static PatientVitalResponse? Create(PatientVital patientVital) 
        {
            if (patientVital == null)
            {
                return null;
            }

            var data = new PatientVitalResponse
            {
                Nurse = StaffResponse.Create(patientVital.Nurse),
                Data = patientVital.Data,
                Base = BaseResponse.Create(patientVital)
            };

            return data;
        }
    }
}
