using Application.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AppointmentEntities.GetAppointments
{
    public class AppointmentResponse
    {
        public BaseResponse? Base { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public CompanyResponse? Company { get; set; }
        public PatientResponse? Patient { get; set; }
        public StaffResponse? Doctor { get; set; }
        public string? OverallDescription { get; set; }

        public static AppointmentResponse Create(AppAppointment appAppointment)
        {
            if (appAppointment == null)
            {
                return null;
            }

            return new AppointmentResponse
            {
                Base = BaseResponse.Create(appAppointment),
                AppointmentDate = appAppointment.AppointmentDate,
                Company = CompanyResponse.Create(appAppointment.Company),
                Patient = PatientResponse.Create(appAppointment.Patient),
                Doctor = StaffResponse.Create(appAppointment.Doctor),
                OverallDescription = appAppointment.OverallDescription,
            };
        }
    }
}
