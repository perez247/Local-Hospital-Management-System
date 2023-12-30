using Application.Interfaces.IRepositories;
using Application.RequestResponsePipeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{

    /// <summary>
    /// Time table controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimeTableController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TimeTableController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

    }
}
