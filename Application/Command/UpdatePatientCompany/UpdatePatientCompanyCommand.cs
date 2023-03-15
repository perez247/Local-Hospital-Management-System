using Application.Annotations;
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

namespace Application.Command.UpdatePatientCompany
{
    public class UpdatePatientCompanyCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
    }

    public class UpdatePatientCompanyHandler : IRequestHandler<UpdatePatientCompanyCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IAuthRepository iAuthRepository;
        private readonly IDBRepository iDBRepository;

        public UpdatePatientCompanyHandler(IUserRepository IUserRepository, IAuthRepository IAuthRepository, IDBRepository IDBRepository)
        {
            iUserRepository = IUserRepository;
            iAuthRepository = IAuthRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdatePatientCompanyCommand request, CancellationToken cancellationToken)
        {
            var ids = new List<string>()
            {
                request.UserId, request.CompanyId
            };

            var users = await iUserRepository.Users()
                                .Include(x => x.Company)   
                                .Include(x => x.Patient)
                                .Where(x => ids.Contains(x.Id.ToString()))
                                .ToListAsync(cancellationToken);

            var patient = users.FirstOrDefault(x => x.Id.ToString() == request.UserId && x.Patient != null);
            
            if (patient == null)
            {
                throw new CustomMessageException("Patient not found");
            }

            var company = users.FirstOrDefault(x => x.Id.ToString() == request.CompanyId && x.Company != null);

            if (company == null)
            {
                throw new CustomMessageException("Company not found");
            }

            patient.Patient.CompanyId = company.Company.Id;

            iDBRepository.Update<Patient>(patient.Patient);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
