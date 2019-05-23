using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Lefkogeia.Server
{
    public class Startup
    {
        private string AppDir => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private string LogDir => Path.Combine(AppDir, "logs");

        private string LogFile => Path.Combine(LogDir, "access.txt");

        private static int LogFileCounter = 0;

        private StreamWriter _LogFile;

        private StreamWriter LogFileAppender
        {
            get
            {
                if (_LogFile == null)
                {
                    _LogFile = File.AppendText(LogFile);
                    _LogFile.AutoFlush = true;
                }
                return _LogFile;
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Run(async (context) =>
            {
                logRequest(context.Request);
                await context.Response.WriteAsync($"Thank you for your " + context.Request.Method);
            });
        }

        private void logRequest(HttpRequest request)
        {
            // don't log favicon requests from browsers
            if (request.Path == "/favicon.ico")
            {
                return;
            }

            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }

            string bodyFileName = (++LogFileCounter).GetFilename(LogDir);
            while (File.Exists(bodyFileName))
            {
                bodyFileName = (++LogFileCounter).GetFilename(LogDir);
            }

            string reqLogEntry = $"{(LogFileCounter).ToString("000000")};{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};{request.Method};{request.Path};{request.QueryString};{request.ContentType};{request.QueryString};";
            using (var reader = new StreamReader(request.Body))
            {
                var body = reader.ReadToEnd();
                File.WriteAllText(bodyFileName, body);
            }
            LogFileAppender.WriteLine(reqLogEntry);
        }
    }
}