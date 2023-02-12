using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetUserList;
using DBService.QueryHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDBContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public PatientRepository(AppDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IQueryable<Patient> Patients()
        {
            return _context.Patients.AsQueryable();
        }

        public async Task<AppUser> CreatePatient(AppUser newUser, string password)
        {
            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                throw new CustomMessageException(result.Errors.FirstOrDefault()?.Description ?? string.Empty);
            }

            return newUser;
        }

        public async Task<PaginationDto<PatientVital>> GetPatientVitals(string patientId, PaginationCommand command)
        {
            var query = _context.PatientVitals
                                .Include(x => x.Nurse)
                                    .ThenInclude(x => x.AppUser)
                                .OrderByDescending(x => x.DateCreated)
                                .Where(x => x.PatientId.ToString() == patientId)
                                .AsQueryable();

            return await query.GenerateEntity(command);
        }
    }
}
