using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Api.Validation.ProblemDetails.Exceptions
{
    public class InputValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; private set; }

        public InputValidationException()
        {
            this.Errors = new Dictionary<string, string[]>();
        }

        public InputValidationException(IDictionary<string, string[]> errors)
        {
            this.Errors = errors;
        }


        public void AddError(string key, string message)
        {
            if (Errors.ContainsKey(key))
            {
                var errors = this.Errors[key];
                errors.Append(message);
                this.Errors[key] = errors;
            }
            else
            {
                this.Errors.Add(key, new string[] { message });
            }
        }
    }
}
