using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Lefkogeia.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .Build();
        }

        public static IWebHostBuilder CreateDefaultBuilder(string[] args)
        {
            var config = GetAppConfig(args);

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
            string url = string.Join(';', urlConfigs.Select(x => x.Value));
            builder.UseUrls(url);
            return builder;
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
