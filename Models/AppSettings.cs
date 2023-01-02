using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppSettings : BaseEntity
    {
        public decimal? PatientRegistrationFee { get; set; } = 1500;
        public decimal? CompanyRegistrationFee { get; set; } = 5000;
    }
}
