using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.ViewStaff
{
    public class ViewStaffQuery : TokenCredentials, IRequest<UserResponse>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
    }

    public class ViewStaffHandler : IRequestHandler<ViewStaffQuery, UserResponse>
    {
        private readonly IUserRepository iUserRepository;
        public ViewStaffHandler(IUserRepository IUserRepository)
        {
            iUserRepository = IUserRepository;
        }

        public async Task<UserResponse> Handle(ViewStaffQuery request, CancellationToken cancellationToken)
        {
            var user = await iUserRepository.Users()
                                            .Include(x => x.Staff)
                                                //.ThenInclude(s => s.SalaryPaymentHistory.OrderBy(y => y.))
                                            .Include(x => x.NextOfKin)
                                            .Include(x => x.UserRoles).ThenInclude(y => y.Role)
                                            .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                throw new CustomMessageException("Staff not found", System.Net.HttpStatusCode.NotFound);
            }

            return UserResponse.Create(user);
        }
    }
}
