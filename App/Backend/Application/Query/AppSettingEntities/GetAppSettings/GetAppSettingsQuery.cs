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

namespace Application.Query.AppSettingEntities.GetAppSettings
{
    public class GetAppSettingsQuery : TokenCredentials, IRequest<IEnumerable<AppSettingResponse>>
    {}

    public class GetAppSettingsHandler : IRequestHandler<GetAppSettingsQuery, IEnumerable<AppSettingResponse>>
    {
        public IAppSettingRepository _settingRepository { get; set; }

        public GetAppSettingsHandler(IAppSettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<IEnumerable<AppSettingResponse>> Handle(GetAppSettingsQuery request, CancellationToken cancellationToken)
        {
            var result = await _settingRepository.AppSettings().ToListAsync();

            return result.Select(x => AppSettingResponse.Create(x));
        }
    }
}
