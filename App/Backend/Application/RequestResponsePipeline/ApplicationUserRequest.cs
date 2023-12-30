using Application.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Models;
using System.Security.Claims;

namespace Application.RequestResponsePipeline
{
    public class ApplicationUserRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public IMediator? Mediator { get; set; }
        public AppUser? CurrentUser { get; set; }
        public HttpContext? HttpContext { get; set; }
        public IUserRepository? UserRepository { get; set; }
        public string? RequestName { get; set; }
    }
}
