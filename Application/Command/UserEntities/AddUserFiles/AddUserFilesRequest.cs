using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UserEntities.AddUserFiles
{
    public class AddUserFilesRequest
    {
        public string? Name { get; set; }
        public string? Base64String { get; set; }
    }
}
