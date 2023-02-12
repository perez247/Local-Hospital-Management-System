using Application.Command.AddCompanyContract;
using Application.Command.CreateCompany;
using Application.Command.UpdateAppCost;
using Application.Command.UpdateCompany;
using Application.Interfaces.IRepositories;
using Application.RequestResponsePipeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Financial controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CompanyController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// create a company
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CreateCompanyResponse), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyCommand command)
        {
            await UpdateToken(command, nameof(CreateCompanyCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Add Company contract
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateCost([FromBody] AddCompanyContractCommand command)
        {
            await UpdateToken(command, nameof(AddCompanyContractCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update Company details
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("details")]
        public async Task<IActionResult> UpdateCompanyDetails([FromBody] UpdateCompanyCommand command)
        {
            await UpdateToken(command, nameof(UpdateCompanyCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}
