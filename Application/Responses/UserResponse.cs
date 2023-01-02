using Application.Query.ViewStaff;
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
        public ViewStaffResponse? Staff { get; set; }
        public NextofKinResponse? NextOfKin { get; set; }
        public ICollection<string> UserRoles { get; set; } = new List<string>();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
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
            data.Address = user.Address;
            data.Staff = ViewStaffResponse.Create(user?.Staff);
            data.NextOfKin = NextofKinResponse.Create(user?.NextOfKin);
            data.UserRoles = user.UserRoles.Select(x => x.Role.Name.ToString()).ToList();
            data.Base = BaseResponse.Create(user);

            return data;
        }
    }
}
