using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Tests.IntegrationTests.Envinroment.Extensions
{
    public static class WebApplicationFactoryExtensions
    {
        public static Task RunWithInjectionAsync<TService>(this VeterinaryFactory factory, Func<TService, Task> action)
        {
            using var scope = factory.Services.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<TService>();
            return action(service);
        }

        public static Task RunWithInjectionAsync<TEntryPoint, TInjectedService1, TInjectedService2>(
            this WebApplicationFactory<TEntryPoint> factory,
            Func<TInjectedService1, TInjectedService2, Task> action)
            where TEntryPoint : class
            where TInjectedService1 : class
            where TInjectedService2 : class
        {
            using (var scope = factory.Services.CreateScope())
            {
                var service1 = scope.ServiceProvider.GetRequiredService<TInjectedService1>();
                var service2 = scope.ServiceProvider.GetRequiredService<TInjectedService2>();

                return action(service1, service2);
            }
        }

        public static Task RunWithInjectionAsync<TEntryPoint, TInjectedService1, TInjectedService2, TInjectedService3>(
            this WebApplicationFactory<TEntryPoint> factory,
            Func<TInjectedService1, TInjectedService2, TInjectedService3, Task> action)
            where TEntryPoint : class
            where TInjectedService1 : class
            where TInjectedService2 : class
            where TInjectedService3 : class
        {
            using (var scope = factory.Services.CreateScope())
            {
                var service1 = scope.ServiceProvider.GetRequiredService<TInjectedService1>();
                var service2 = scope.ServiceProvider.GetRequiredService<TInjectedService2>();
                var service3 = scope.ServiceProvider.GetRequiredService<TInjectedService3>();

                return action(service1, service2, service3);
            }
        }
    }
}
