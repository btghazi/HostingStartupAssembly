using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HostingAssemblyDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            var pluginName = GetPluginAssemblyName();
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                                .UseSetting(WebHostDefaults.HostingStartupAssembliesKey, pluginName)
                                .UseStartup<Startup>();

                });
        }

        public static string GetPluginAssemblyName()
        {
            Console.WriteLine($"Launched from {Environment.CurrentDirectory}");
            Console.WriteLine($"Physical location {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"AppContext.BaseDir {AppContext.BaseDirectory}");
            Console.WriteLine($"Runtime Call {Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}");

            var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var pluginNames = directoryInfo.GetFiles("PlugIn*.dll").Select(potentialPspPlugin => Path.GetFileNameWithoutExtension(potentialPspPlugin.Name));
            // In case of many plug-ins or there is no plug-in at all, we should throw exception and abort.
            if (!pluginNames.Any() || pluginNames.Skip(1).Any())
            {
                throw new ApplicationException("It's seems that there is more than on plug-in that are identified.");
            }

            return pluginNames.First();
        }

    }
}
