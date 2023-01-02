using Application.Query.ViewStaff;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class NextofKinResponse
    {
        public Guid? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public BaseResponse? Base { get; set; }

        public static NextofKinResponse? Create(NextOfKin nextOfKin)
        {
            if (nextOfKin == null)
            {
                return null;
            }

            var data = new NextofKinResponse();
            data.UserId = nextOfKin.AppUserId;
            data.FirstName = nextOfKin.FirstName;
            data.LastName = nextOfKin.LastName;
            data.OtherName = nextOfKin.OtherName;
            data.Phone1 = nextOfKin.Phone1;
            data.Phone2 = nextOfKin.Phone2;
            data.Email = nextOfKin.Email;
            data.Address = nextOfKin.Address;
            data.Base = BaseResponse.Create(nextOfKin);

            return data;
        }
    }
}
