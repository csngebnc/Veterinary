using System;

namespace Veterinary.Tests.IntegrationTests.Envinroment.ErrorHandling
{
    public class TestAssertException : Exception
    {
        public TestAssertException(string message)
            : base(message)
        {
        }
    }
}
