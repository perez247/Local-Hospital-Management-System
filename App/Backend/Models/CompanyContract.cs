using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CompanyContract : BaseContract
    {
        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
