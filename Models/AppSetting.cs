using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppSetting : BaseEntity
    {
        public AppSettingType Type { get; set; }
        public string? Data { get; set; }
    }

    public class AppSettingBilling
    {
        public decimal PatientRegistrationFee { get; set; } = 1500m;
        public decimal CompanyRegistrationFee { get; set; } = 5000m;
        public decimal Tax { get; set; } = 0.010m;
    }
}
