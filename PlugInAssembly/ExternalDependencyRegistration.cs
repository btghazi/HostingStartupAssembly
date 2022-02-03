using Citron.SDK;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[assembly: HostingStartup(typeof(PlugInAssembly.ExternalDependencyRegistration))]

namespace PlugInAssembly
{
    public class ExternalDependencyRegistration : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine("Call configure from external assembly.");

            var hello = Class1.Hello();
            var helloAsJson = JsonConvert.SerializeObject(hello);

            builder.ConfigureAppConfiguration(config =>
            {
                var dict = new Dictionary<string, string>
                {
                    {"DevAccount_FromLibrary", "DEV_1111111-1111"},
                    {"ProdAccount_FromLibrary", "PROD_2222222-2222"}
                };

                config.AddInMemoryCollection(dict);
            });
        }
    }
}