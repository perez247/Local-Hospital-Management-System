using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class UserOnlyResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public bool IsPatient { get; set; }
        public bool IsCompany { get; set; }
        public BaseResponse? Base { get; set; }

        public static UserOnlyResponse? Create(AppUser user)
        {
            if (user == null)
            {
                return null;
            }

            var data = new UserOnlyResponse();
            data.FirstName = user.FirstName;
            data.LastName = user.LastName;
            data.OtherName = user.OtherName;
            data.Phone = user.PhoneNumber;
            data.Email = user.Email;
            data.Address = user.Address;
            data.Profile = user.Profile;
            data.IsPatient = user.Patient != null;
            data.IsCompany = user.Company != null;
            data.Base = BaseResponse.Create(user);

            return data;
        }
    }
}
