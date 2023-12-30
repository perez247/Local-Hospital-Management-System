using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class AppSettingResponse
    {
        public BaseResponse? Base { get; set; }
        public string? Type { get; set; }
        public string? Data { get; set; }

        public static AppSettingResponse? Create(AppSetting appSetting)
        {
            if (appSetting == null)
            {
                return null;
            }

            return new AppSettingResponse
            {
                Base = BaseResponse.Create(appSetting),
                Type = appSetting.Type.ToString(),
                Data = appSetting.Data,
            };
        }
    }
}
