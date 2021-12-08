using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Tests.IntegrationTests.Envinroment;

namespace Veterinary.Tests.IntegrationTests.Tests
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected HttpClient client;
        protected VeterinaryFactory factory;

        public IntegrationTestBase()
        {
            factory = new VeterinaryFactory();
            client = factory.CreateClient();
        }

        public void Dispose()
        {
            client.Dispose();
            factory.Dispose();
        }
    }
}
