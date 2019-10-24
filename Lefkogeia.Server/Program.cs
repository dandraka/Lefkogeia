using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Lefkogeia.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static string CurrentDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string CertDir => Path.Combine(CurrentDir, "certs");

        public static IWebHost BuildWebHost(string[] args)
        {
            return CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .Build();
        }

        public static IWebHostBuilder CreateDefaultBuilder(string[] args)
        {
            var config = GetAppConfig(args);

            var certs = GetCertsAvailable();

            var builder = new WebHostBuilder()
                .UseKestrel()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                });

            var urlConfigs = config.GetSection("Host:Url").GetChildren();
            //string url = string.Join(';', urlConfigs.Select(x => x.Value));
            //builder.UseUrls(url);
            
                builder.ConfigureKestrel((context, options) => {
                    foreach (var url in urlConfigs.Select(x => x.Value))
                    {
                        var uri = new Uri(url);
                        options.Listen(IPAddress.Parse(uri.Host), uri.Port, listenOptions =>
                        {
                            if (uri.Scheme == "https")
                            {
                                if (certs.Contains(c => Path.GetFileNameWithoutExtension(c).ToLower() == uri.Host.ToLower()))

                                listenOptions.UseHttps();
                            }
                        });
                    }
                });
            return builder;
        }

        private static List<string> GetCertsAvailable()
        {
            return Directory.EnumerateFiles(CertDir, "*.pfx").ToList();
        }

        private static IConfigurationRoot GetAppConfig(string[] args)
        {
            var appConfigBuilder = new ConfigurationBuilder();
            appConfigBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (args != null)
            {
                appConfigBuilder.AddCommandLine(args);
            };
            return appConfigBuilder.Build();
        }
    }
}
