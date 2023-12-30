using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.AppSettingEntities.UpdateSetting;
using Application.Interfaces.IRepositories;
using Application.Query.AppSettingEntities.GetAppSettings;
using Application.RequestResponsePipeline;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Setting controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Get all the settings
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<AppSettingResponse>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> AddAppointment([FromQuery] GetAppSettingsQuery command)
        {
            await UpdateToken(command, nameof(GetAppSettingsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update settings
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPatch]
        public async Task<IActionResult> UpdateSetting([FromBody] UpdateSettingCommand command)
        {
            await UpdateToken(command, nameof(UpdateSettingCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}
