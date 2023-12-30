using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class UserResponse
    {
        public StaffResponse? Staff { get; set; }
        public PatientResponse? Patient { get; set; }
        public CompanyResponse? Company { get; set; }
        public NextofKinResponse? NextOfKin { get; set; }
        public ICollection<string> UserRoles { get; set; } = new List<string>();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public BaseResponse? Base { get; set; }

        public static UserResponse? Create(AppUser user)
        {
            if (user == null)
            {
                return null;
            }

            var data = new UserResponse();
            data.FirstName = user.FirstName;
            data.LastName = user.LastName;
            data.OtherName = user.OtherName;
            data.Phone = user.PhoneNumber;
            data.Email = user.Email;
            data.Address = user.Address;
            data.Profile = user.Profile;
            data.Staff = StaffResponse.Create(user?.Staff);
            data.Company = CompanyResponse.Create(user?.Company);
            data.Patient = PatientResponse.Create(user?.Patient);
            data.NextOfKin = NextofKinResponse.Create(user?.NextOfKin);
            data.UserRoles = user.UserRoles.Select(x => x.Role.Name.ToString()).ToList();
            data.Base = BaseResponse.Create(user);

            return data;
        }
    }
}
