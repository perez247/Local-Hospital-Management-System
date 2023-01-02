using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateCompany
{
    public class CreateCompanyCommand : TokenCredentials, IRequest<CreateCompanyResponse>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? UniqueId { get; set; }
        public string? OtherId { get; set; }
    }

    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
    {
        private readonly IAuthRepository iAuthRepository;
        private readonly ICompanyRepository iCompanyRepository;

        public CreateCompanyHandler(IAuthRepository IAuthRepository, ICompanyRepository ICompanyRepository)
        {
            iAuthRepository = IAuthRepository;
            iCompanyRepository = ICompanyRepository;
        }
        public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var emailAvaliable = await iAuthRepository.IsEmailAvailable(request.Email);

            if (!emailAvaliable)
            {
                throw new CustomMessageException($"{request.Email} has been taken");
            }

            AppUser newUser = new AppUser
            {
                FirstName = request.Name,
                LastName = request.Name,
                Address = request.Address,
                Email = request.Email,
                UserName = request.Email,
                Company = new Company
                {
                    Id = Guid.NewGuid(),
                    Description = request.Description,
                    UniqueId = request.UniqueId,
                    OtherId = request.OtherId,
                }
            };

            string password = UtilityHelper.GenerateRandomPassword();

            newUser = await iCompanyRepository.CreateCompany(newUser, password);

            return new CreateCompanyResponse
            {
                CompanyId = newUser?.Company.Id?.ToString() ?? string.Empty
            };
        }
    }
}
