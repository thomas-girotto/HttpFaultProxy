using System.Net.Http;
using HttpFaultProxy.Middleware;
using HttpFaultProxy.Model.Proxies;
using HttpFaultProxy.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HttpFaultProxy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            HttpClient.DefaultProxy = new NoProxy();
            services.AddSingleton(new HttpClient());
            services.AddSingleton<ProxyFactory>();
            services.AddSingleton<IProxyProvider, ProxyProvider>();
            services.AddSingleton<IProxy, ForwardProxy>();
            services.AddSingleton<IScheduler, Scheduler>();
            services
                .AddOptions<ProxyOptions>("Routes")
                .BindConfiguration(ProxyOptions.SectionName);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ReverseProxyMiddleware>(app.ApplicationServices.GetRequiredService<IProxyProvider>());
        }
    }
}
