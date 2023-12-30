using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class PatientContractResponse
    {
        public virtual AppCostResponse? AppCost { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public PatientResponse? Patient { get; set; }

        public static PatientContractResponse? Create(PatientContract patientContract)
        {
            if (patientContract == null)
            {
                return null;
            }

            return new PatientContractResponse
            {
                AppCost = AppCostResponse.Create(patientContract.AppCost),
                StartDate = patientContract.StartDate,
                Duration = patientContract.Duration,
            };
        }
    }

}
