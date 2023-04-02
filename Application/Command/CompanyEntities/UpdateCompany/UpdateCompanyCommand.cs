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

namespace Application.Command.CompanyEntities.UpdateCompany
{
    public class UpdateCompanyCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? UniqueId { get; set; }
        public string? OtherId { get; set; }
        public string? Profile { get; set; }
        public bool ForIndividual { get; set; }
        public bool HomeCompany { get; set; }
    }

    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Unit>
    {
        private readonly ICompanyRepository iCompanyRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateCompanyHandler(IDBRepository IDBRepository, ICompanyRepository ICompanyRepository)
        {
            iDBRepository = IDBRepository;
            iCompanyRepository = ICompanyRepository;
        }
        public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {


            var company = await iCompanyRepository.Companies()
                                                  .Include(x => x.AppUser)
                                                  .Include(x => x.CompanyContracts)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);

            if (company == null)
            {
                throw new CustomMessageException($"Company to update not found");
            }

            if (!request.HomeCompany && company.HomeCompany)
            {
                throw new CustomMessageException($"Company '{company.AppUser.FirstName}' cannot be removed as the home company");
            }

            if (!request.ForIndividual && company.ForIndividual)
            {
                throw new CustomMessageException($"'{company.AppUser.FirstName}' cannot be removed as the company for individuals");
            }

            if (request.HomeCompany && !company.HomeCompany)
            {
                var homeCompany = await iCompanyRepository.Companies()
                                           .Include(x => x.AppUser)
                                           .FirstOrDefaultAsync(x => x.HomeCompany);

                if (homeCompany != null)
                {
                    throw new CustomMessageException($"'{homeCompany.AppUser.FirstName}' is already the home company cannot be removed as the home company");
                }

                if (company.ForIndividual)
                {
                    throw new CustomMessageException($"'{company.AppUser.FirstName}' cannot be made as home company because it is already for individuals");
                }
            }

            if (request.ForIndividual && !company.ForIndividual)
            {
                var individualCompany = await iCompanyRepository.Companies()
                                               .Include(x => x.AppUser)
                                               .FirstOrDefaultAsync(x => x.ForIndividual);

                if (individualCompany != null)
                {
                    throw new CustomMessageException($"'{individualCompany}' is already the company for individuals cannot be removed");
                }
            }

            var user = company.AppUser;

            user.FirstName = request.Name;
            user.Address = request.Address;
            user.Profile = request.Profile;

            company.Description = request.Description;
            company.UniqueId = request.UniqueId;
            company.OtherId = request.OtherId;
            company.ForIndividual = request.ForIndividual;
            company.HomeCompany = request.HomeCompany;

            if ((company.ForIndividual || company.HomeCompany) && company.CompanyContracts.Count > 0)
            {
                foreach (var contract in company.CompanyContracts)
                {
                    iDBRepository.Remove<CompanyContract>(contract);
                }
            }

            iDBRepository.Update(user);
            iDBRepository.Update(company);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
