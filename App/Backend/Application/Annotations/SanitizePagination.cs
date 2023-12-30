using Application.Paginations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Annotations
{
    /// <summary>
    /// Annotation to mainly verify the Guid or set a default value if not a valid one
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SanitizePagination : ValidationAttribute
    {
        /// <summary>
        /// Constructor: Annotation to mainly verify the Guid or set a default empty Guid if its an invalid Guid
        /// </summary>
        public SanitizePagination() : base("")
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


            var propertyValue = (PaginationCommand)thisProperty.GetValue(validationContext.ObjectInstance, null);

            if (propertyValue != null)
                return null;

            var newValue = new PaginationCommand() { PageNumber = 1, PageSize = 10 };

            thisProperty.SetValue(validationContext.ObjectInstance, newValue);
            return null;

        }
    }
}
