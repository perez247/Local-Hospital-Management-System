﻿using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.PatientEntities.CreatePatient
{
    public class CreatePatientCommand : TokenCredentials, IRequest<CreatePatientResponse>
    {
        [VerifyGuidAnnotation]
        public string? StaffId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
    }

    public class CreatePaitentHandler : IRequestHandler<CreatePatientCommand, CreatePatientResponse>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;
        private readonly IAuthRepository iAuthRepository;
        private readonly ICompanyRepository iCompanyRepository;

        public CreatePaitentHandler(IPatientRepository IPatientRepository, IAuthRepository IAuthRepository, IStaffRepository IStaffRepository, IDBRepository IDBRepository, ICompanyRepository ICompanyRepository)
        {
            iPatientRepository = IPatientRepository;
            iAuthRepository = IAuthRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
            iCompanyRepository = ICompanyRepository;
        }
        public async Task<CreatePatientResponse> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {

            if (request.StaffId != Guid.Empty.ToString())
            {
                var staff = await iStaffRepository.Staff()
                                                  .Include(x => x.AppUser)
                                                    .ThenInclude(x => x.Patient)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.StaffId);

                if (staff == null)
                {
                    throw new CustomMessageException("Staff to add as patient not found");
                }

                var homeCompany = await iCompanyRepository.Companies()
                                .FirstOrDefaultAsync(x => x.HomeCompany);

                if (homeCompany == null)
                {
                    throw new CustomMessageException("Home company is required for adding staff as a patient");
                }

                var patient = staff.AppUser.Patient;

                if (patient != null)
                {
                    throw new CustomMessageException("Staff is already a patient");
                }

                var newPatient = new Patient
                {
                    Id = Guid.NewGuid(),
                    AppUserId = staff.AppUserId,
                    CompanyId = homeCompany.Id
                };

                await iDBRepository.AddAsync(newPatient);
                await iDBRepository.Complete();

                return new CreatePatientResponse { UserId = staff.AppUserId?.ToString() ?? string.Empty };
            }
            else
            {
                var emailAvaliable = await iAuthRepository.IsEmailAvailable(request.Email);

                if (!emailAvaliable)
                {
                    throw new CustomMessageException($"{request.Email} has been taken");
                }

                var individualCompany = await iCompanyRepository.Companies()
                                .FirstOrDefaultAsync(x => x.ForIndividual);

                if (individualCompany == null)
                {
                    throw new CustomMessageException("A Company for individual must be created first");
                }

                //var staff = await iStaffRepository.Staff()
                //                                  .Include(x => x.)

                AppUser newUser = new AppUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    OtherName = request.OtherName,
                    Address = request.Address,
                    Email = request.Email,
                    UserName = request.Email,
                    Patient = new Patient
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = individualCompany.Id,
                        //CompanyId = request.CompanyId == Guid.Empty.ToString() ? null : Guid.Parse(request.CompanyId),
                    },
                };

                string password = UtilityHelper.GenerateRandomPassword();

                newUser = await iPatientRepository.CreatePatient(newUser, password);

                // You can send patient the username and password

                return new CreatePatientResponse { 
                    UserId = newUser?.Id.ToString() ?? string.Empty,
                    Password = password,
                    Email = request.Email,
                    FullName = $"{request.LastName} {request.FirstName}"
                };
            }

        }
    }
}
