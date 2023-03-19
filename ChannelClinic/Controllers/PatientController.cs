using Application.Command.PatientEntities.AddPatientContract;
using Application.Command.PatientEntities.AddPatientVital;
using Application.Command.PatientEntities.CreatePatient;
using Application.Command.PatientEntities.UpdatePatientAllergy;
using Application.Command.PatientEntities.UpdatePatientCompany;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.PatientEntities.PatientVitals;
using Application.RequestResponsePipeline;
using Application.Responses;
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


        /// <summary>
        /// Get a list of patients vitals
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<PatientVitalResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("vitals")]
        public async Task<IActionResult> GetPatientVitals([FromBody] PatientVitalsQuery command)
        {
            await UpdateToken(command, nameof(PatientVitalsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update Allergies
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreatePatientResponse), (int)HttpStatusCode.OK)]
        [HttpPut("allergies")]
        public async Task<IActionResult> UpdatePatientAllergies([FromBody] UpdatePatientAllergyCommand command)
        {
            await UpdateToken(command, nameof(UpdatePatientAllergyCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Add Patient Contract
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("contract")]
        public async Task<IActionResult> AddPatientContract([FromBody] AddPatientContractCommand command)
        {
            await UpdateToken(command, nameof(AddPatientContractCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update the patients company
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("patient-company")]
        public async Task<IActionResult> UpdatePatientCompany([FromBody] UpdatePatientCompanyCommand command)
        {
            await UpdateToken(command, nameof(UpdatePatientCompanyCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

    }
}
