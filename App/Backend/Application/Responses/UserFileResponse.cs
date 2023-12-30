using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class UserFileResponse
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Base64String { get; set; }

        public static UserFileResponse? Create(UserFile userFile)
        {
            if (userFile == null)
            {
                return null;
            }

            return new UserFileResponse
            {
                Id = userFile.Id.ToString(),
                UserId = userFile.AppUserId.ToString(),
                Name = userFile.Name,
                Base64String = userFile.Base64String,
            };
        }
    }
}
