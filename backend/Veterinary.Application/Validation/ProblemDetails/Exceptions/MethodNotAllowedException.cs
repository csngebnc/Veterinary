using System;
using System.Runtime.Serialization;

namespace Veterinary.Application.Validation.ProblemDetails.Exceptions
{
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException()
        {
        }

        public MethodNotAllowedException(string message) : base(message)
        {
        }

        public MethodNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MethodNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
