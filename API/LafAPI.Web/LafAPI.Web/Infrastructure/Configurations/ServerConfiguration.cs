namespace LafAPI.Web.Infrastructure.Configurations
{
    using System;

    using LafAPI.Web.Hubs;

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
                .UseRouting()
                .UseCors("CorsPolicy"/*x => x.AllowCredentials().AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()*/)
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute("default", "{controller}/{action}/{id?}");
                        endpoints.MapHub<ChatHub>("/chat");
                    });

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
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
                .AddCors(
                     options =>
                         {
                             options.AddPolicy(
                                 "CorsPolicy",
                                 builder => builder.WithOrigins(
                                         "http://localhost:4200",
                                         "https://localhost:4200",
                                         "http://0.0.0.0:4200",
                                         "https://0.0.0.0:4200",
                                         "http://127.0.0.1:4200",
                                         "https://127.0.0.1:4200",
                                         "http://192.168.0.33:4200",
                                         "https://192.168.0.33:4200")
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .AllowCredentials());
                         })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }
    }
}