namespace LafAPI.Web.Infrastructure.Configurations
{
    using System;

    using LafAPI.Web.Hubs;
    using LafAPI.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using static LafAPI.Web.Infrastructure.Common.ApplicationConstants;

    public class ServerConfiguration : BaseConfiguration
    {
        private const string CorsPolicy = nameof(CorsPolicy);
        private readonly string[] origins;

        public ServerConfiguration(IConfiguration configuration) : base(configuration)
            => this.origins = new[]
                               {
                                   "http://localhost:4200",
                                   "https://localhost:4200",
                                   "http://0.0.0.0:4200",
                                   "https://0.0.0.0:4200",
                                   "http://127.0.0.1:4200",
                                   "https://127.0.0.1:4200",
                                   "http://192.168.0.33:4200",
                                   "https://192.168.0.33:4200"
                               };

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) =>
            app.UseHsts()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(CorsPolicy)
                .ConfigureWithAntiforgery(app.ApplicationServices.GetRequiredService<IAntiforgery>())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute("default", "{controller}/{action}/{id?}");
                        endpoints.MapHub<ChatHub>("/chat");
                    })
                .UseSession();

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.Configure<CookiePolicyOptions>(options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.Strict;
                    })
                .AddDistributedMemoryCache()
                .AddSession(options =>
                    {
                        options.Cookie.Name = ".LafAPI.Session";
                        // Set a short timeout for easy testing.
                        options.IdleTimeout = TimeSpan.FromSeconds(10);
                        options.Cookie.HttpOnly = false;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                        // Make the session cookie essential
                        options.Cookie.IsEssential = true;
                    })
                 .AddAntiforgery(options =>
                     {
                         // Set Cookie properties using CookieBuilder properties†.
                         options.HeaderName = XSRF;
                         options.FormFieldName = "RequestVerificationToken";
                         options.Cookie.HttpOnly = false;
                         options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                         options.Cookie.IsEssential = true;
                         options.SuppressXFrameOptionsHeader = false;
                     })
                .AddHsts(options =>
                    {
                        options.Preload = true;
                        options.IncludeSubDomains = true;
                        options.MaxAge = TimeSpan.FromDays(60);
                        // options.ExcludedHosts.Add("example.com");
                        // options.ExcludedHosts.Add("www.example.com");
                    })
                .AddHttpsRedirection(options =>
                    {
                        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                        options.HttpsPort = int.Parse(this.Configuration["Kestrel:Ports:https"]);
                    })
                .AddCors(options =>
                    {
                        options.AddPolicy(
                            CorsPolicy,
                            builder =>
                                builder.WithOrigins(this.origins)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
                    })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }
    }
}