using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Api.Validation.ProblemDetails.Data
{
    public class InputValidationErrors : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public Dictionary<string, ICollection<string>> Errors { get; set; }

        public InputValidationErrors(IEnumerable<ValidationFailure> errors)
        {
            this.Errors = new Dictionary<string, ICollection<string>>();

            foreach (var failure in errors)
            {
                AddError(failure.PropertyName, failure.ErrorMessage);
            }

        }

        public void AddError(string key, string message)
        {
            if (Errors.ContainsKey(key))
            {
                var errors = this.Errors[key];
                errors.Add(message);
                this.Errors[key] = errors;
            }
            else
            {
                var errors = new List<string>();
                errors.Add(message);
                this.Errors.Add(key, errors);
            }
        }
    }
}
