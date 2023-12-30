using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class VerifyGuidAnnotation : ValidationAttribute
    {
        /// <summary>
        /// Constructor: Annotation to mainly verify the Guid or set a default empty Guid if its an invalid Guid
        /// </summary>
        public VerifyGuidAnnotation() : base("")
        {

        }

        /// <summary>
        /// Valid the field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var thisProperty = validationContext.ObjectType.GetProperty(validationContext.MemberName);


            var propertyValue = (string)thisProperty.GetValue(validationContext.ObjectInstance, null);

            if (Guid.TryParse(propertyValue, out Guid result))
                return null;

            thisProperty.SetValue(validationContext.ObjectInstance, Guid.Empty.ToString());
            return null;

        }
    }
}
