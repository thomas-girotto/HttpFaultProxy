using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HttpFaultProxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("HTTP_PROXY", "http://localhost:5001", EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("HTTPS_PROXY", "http://localhost:5001", EnvironmentVariableTarget.User);

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                Environment.SetEnvironmentVariable("HTTP_PROXY", null, EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("HTTPS_PROXY", null, EnvironmentVariableTarget.User);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
