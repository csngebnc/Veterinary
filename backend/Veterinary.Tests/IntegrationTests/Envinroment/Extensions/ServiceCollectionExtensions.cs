using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Tests.IntegrationTests.Envinroment.Authentication;

namespace Veterinary.Tests.IntegrationTests.Envinroment.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddCustomAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<IntegrationTestAuthenticationOptions, IntegrationTestAuthenticationMiddleware>("Bearer", options => { });
        }
    }
}
