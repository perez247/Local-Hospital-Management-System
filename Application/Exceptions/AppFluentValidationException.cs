using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AppFluentValidationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppFluentValidationException() : base("One or more")
        {
            Failures = new List<CustomValidationFailure>();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="failures"></param>
        public AppFluentValidationException(IList<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(new CustomValidationFailure
                {
                    FieldName = propertyName,
                    FieldErrors = propertyFailures
                });
            }
        }

        /// <summary>
        /// List of errors
        /// </summary>
        /// <value></value>
        public ICollection<CustomValidationFailure> Failures { get; }
    }

    /// <summary>
    /// Validation failure used in this application
    /// </summary>
    public class CustomValidationFailure
    {
        /// <summary>
        /// Field that has the error
        /// </summary>
        /// <value></value>
        public string? FieldName { get; set; }

        /// <summary>
        /// Errors of the field
        /// </summary>
        /// <value></value>
        public string[]? FieldErrors { get; set; }
    }
}