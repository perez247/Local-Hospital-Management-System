using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Annotations
{
    /// <summary>
    /// Defines the pagination size
    /// </summary>    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumberRangeAnnotation : ValidationAttribute
    {
        private readonly int _lowest;
        private readonly int _highest;
        private readonly int _defaultValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Lowest"></param>
        /// <param name="Highest"></param>
        /// <param name="DefaultValue"></param>
        public NumberRangeAnnotation(int Lowest, int Highest, int DefaultValue)
        : base("")
        {
            _lowest = Lowest;
            _highest = Highest;
            _defaultValue = DefaultValue;
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

            // if (value == null){
            //     thisProperty.SetValue(validationContext.ObjectInstance, 1);
            //     return null;
            // }


            var propertyValue = (int?)thisProperty.GetValue(validationContext.ObjectInstance, null);

            if (propertyValue.HasValue && propertyValue.Value >= _lowest && propertyValue.Value <= _highest)
                return null;

            thisProperty.SetValue(validationContext.ObjectInstance, _defaultValue);
            return null;

        }
    }
}
