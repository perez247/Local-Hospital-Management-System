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

namespace Application.Command.UpdateCompany
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
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.CompanyId);

            if (company == null)
            {
                throw new CustomMessageException($"Company to update not found");
            }

            var user = company.AppUser;

            user.FirstName = request.Name;
            user.Address = request.Address;
            user.Profile = request.Profile;

            company.Description = request.Description;
            company.UniqueId = request.UniqueId;
            company.OtherId = request.OtherId;

            iDBRepository.Update<AppUser>(user);
            iDBRepository.Update<Company>(company);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
