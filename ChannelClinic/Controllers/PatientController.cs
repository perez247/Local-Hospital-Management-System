using Application.Command.AddPatientVital;
using Application.Command.CreatePatient;
using Application.Command.CreateStaff;
using Application.Interfaces.IRepositories;
using Application.RequestResponsePipeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Patient controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : BaseController
    {        
        /// <summary>
        /// Constructor
        /// </summary>
        public PatientController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Create a new staff
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreatePatientResponse), (int)HttpStatusCode.OK)]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command)
        {
            await UpdateToken(command, nameof(CreatePatientCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Add patients vital
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreatePatientResponse), (int)HttpStatusCode.OK)]
        [HttpPost("vital")]
        public async Task<IActionResult> AddPatientVital([FromBody] AddPatientVitalCommand command)
        {
            await UpdateToken(command, nameof(AddPatientVitalCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

    }
}
