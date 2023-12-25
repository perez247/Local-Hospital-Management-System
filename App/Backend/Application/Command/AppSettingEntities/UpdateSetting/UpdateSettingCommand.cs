using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.AppSettingEntities.UpdateSetting
{
    public class UpdateSettingCommand : TokenCredentials, IRequest<Unit>
    {
        public string? AppSettingType { get; set; }
        public string? Data { get; set; }
    }

    public class UpdateSettingHandler : IRequestHandler<UpdateSettingCommand, Unit>
    {
        public IAppSettingRepository _settingRepository { get; set; }
        public IDBRepository _dBRepository { get; set; }

        public UpdateSettingHandler(IAppSettingRepository settingRepository, IDBRepository dBRepository)
        {
            _settingRepository = settingRepository;
            _dBRepository = dBRepository;
        }

        public async Task<Unit> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
        {
            var type = request.AppSettingType.ParseEnum<AppSettingType>();

            var setting = await _settingRepository.AppSettings()
                                                  .FirstOrDefaultAsync(x => x.Type == type);

            if (setting == null)
            {
                throw new CustomMessageException("Settings no found");
            }

            switch (type)
            {
                case AppSettingType.billings:
                    UpdateBilling(request, setting);
                    break;
                default:
                    throw new CustomMessageException("Wrong setting type selected");
            }

            await _dBRepository.Complete();

            return Unit.Value;
        }

        private void UpdateBilling(UpdateSettingCommand request, AppSetting? setting)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<AppSettingBilling>(request.Data);
                var dataFromDB = JsonConvert.DeserializeObject<AppSettingBilling>(setting.Data);

                dataFromDB.PatientRegistrationFee = data.PatientRegistrationFee;
                dataFromDB.CompanyRegistrationFee = data.CompanyRegistrationFee;
                dataFromDB.Tax = data.Tax;

                setting.Data = JsonConvert.SerializeObject(dataFromDB);

                _dBRepository.Update<AppSetting>(setting);
            }
            catch (Exception)
            {
                throw new CustomMessageException("Failed to update billing settings, try again later");
            }
        }
    }
}
