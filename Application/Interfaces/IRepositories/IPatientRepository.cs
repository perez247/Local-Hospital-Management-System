using Application.Paginations;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IPatientRepository
    {
        IQueryable<Patient> Patients();
        Task<AppUser> CreatePatient(AppUser newUser, string password);
        Task<PaginationDto<PatientVital>> GetPatientVitals(string patientId, PaginationCommand command);
    }
}
