using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Veterinary.Tests.IntegrationTests.Envinroment.ErrorHandling; 

namespace Veterinary.Tests.IntegrationTests.Envinroment.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task AssertSuccessAsync(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var message = $"Expected the response to be successful, but got {response.StatusCode} ({(int)response.StatusCode}).\nResponse:\n{content}";

                throw new TestAssertException(message);
            }
        }

        public static async Task AssertStatusCodeAsync(this HttpResponseMessage response, HttpStatusCode statusCode)
        {
            if (response.StatusCode != statusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var message = $"Expected the response status code to be {statusCode}, but got {response.StatusCode} ({(int)response.StatusCode}).\nResponse:\n{content}";

                throw new TestAssertException(message);
            }
        }
    }
}
