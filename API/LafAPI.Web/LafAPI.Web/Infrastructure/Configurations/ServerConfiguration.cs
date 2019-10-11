namespace LafAPI.Web.Infrastructure.Configurations
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ServerConfiguration : BaseConfiguration
    {
        public ServerConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) =>
            app.UseHsts()
                .UseHttpsRedirection()
                .UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
                .UseMvc(routes => routes.MapRoute("default", "{controller}/{action}/{id?}"));

        public override void ConfigureServices(IServiceCollection services) =>
            services.AddHsts(options =>
                    {
                        options.Preload = true;
                        options.IncludeSubDomains = true;
                        options.MaxAge = TimeSpan.FromDays(60);
                        // options.ExcludedHosts.Add("example.com");
                        // options.ExcludedHosts.Add("www.example.com");
                    })
                .AddHttpsRedirection(
                    options =>
                        {
                            options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                            options.HttpsPort = int.Parse(this.Configuration["Kestrel:Ports:https"]);
                        })
                .AddCors()
                .AddMvc(opts => opts.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
    }
}