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
    public class VerifyGuidCollectionAnnotation : ValidationAttribute
    {
        /// <summary>
        /// Constructor: Annotation to mainly verify the Guid or set a default empty Guid if its an invalid Guid
        /// </summary>
        public VerifyGuidCollectionAnnotation() : base("")
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

            var propertyValue = (List<string>)thisProperty.GetValue(validationContext.ObjectInstance, null);

            if (propertyValue == null || propertyValue.Count <= 0)
            {
                thisProperty.SetValue(validationContext.ObjectInstance, new List<string>());
                propertyValue = new List<string>();
            }

            var data = new List<string>();

            foreach (var item in propertyValue)
            {
                if (Guid.TryParse(item, out Guid result))
                    data.Add(item);
                else
                    data.Add(Guid.Empty.ToString());
            }

            thisProperty.SetValue(validationContext.ObjectInstance, data);
            return null;

        }
    }
}
