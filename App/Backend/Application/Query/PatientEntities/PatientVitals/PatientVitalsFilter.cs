using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.PatientEntities.PatientVitals
{
    public class PatientVitalsFilter
    {
        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
    }
}
