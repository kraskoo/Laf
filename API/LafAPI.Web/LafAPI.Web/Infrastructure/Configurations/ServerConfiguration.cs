namespace LafAPI.Web.Infrastructure.Configurations
{
    using System;

    using LafAPI.Web.Hubs;
    using LafAPI.Web.Infrastructure.Common;
    using LafAPI.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using static LafAPI.Web.Infrastructure.Common.ApplicationConstants;

    public class ServerConfiguration : BaseConfiguration
    {
        private const int TwoHundredMegabytes = 200 * 1024 * 1024;
        private const string CorsPolicy = nameof(CorsPolicy);
        private readonly string[] origins;

        public ServerConfiguration(IConfiguration configuration) : base(configuration)
            => this.origins = Providers.CreateDomainsByFourth32Bytes(
                       port: 4200,
                       fourthBytes: new[] { 33, 101, 102, 103, 104, 105, 106, 107 });

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) =>
            app.UseHsts()
                .UseHttpsRedirection()
                .UseFileServer()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseRouting()
                .UseCors(CorsPolicy)
                .UseCookiePolicy()
                .UseSession()
                .ConfigureAuthentication()
                .UseAuthorization()
                .ConfigureWithAntiforgery(app.ApplicationServices.GetRequiredService<IAntiforgery>())
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapHealthChecks("/health");
                        endpoints.MapDefaultControllerRoute()
                            .RequireAuthorization(JwtBearerDefaults.AuthenticationScheme);
                        // .RequireCors(CorsPolicy);
                    });

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHsts(options =>
                    {
                        options.Preload = true;
                        options.IncludeSubDomains = true;
                        options.MaxAge = TimeSpan.FromDays(30);
                    })
                .AddHttpsRedirection(options =>
                    {
                        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                        options.HttpsPort = int.Parse(this.Configuration["Kestrel:Ports:https"]);
                    })
                .Configure<FormOptions>(options =>
                    {
                        options.MultipartBodyLengthLimit = TwoHundredMegabytes;
                    })
                .AddRouting()
                .AddCors(options =>
                    {
                        options.AddPolicy(
                            CorsPolicy,
                            builder =>
                                builder.WithOrigins(this.origins)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials()
                                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                                    .SetPreflightMaxAge(TimeSpan.FromDays(1095))
                                    .Build());
                    })
                .Configure<CookiePolicyOptions>(options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    })
                .AddDistributedMemoryCache()
                .AddSession(options =>
                    {
                        options.Cookie.Name = ".LafAPI.Session";
                        options.IdleTimeout = TimeSpan.FromSeconds(10);
                        options.Cookie.HttpOnly = false;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                        options.Cookie.IsEssential = true;
                    })
                .AddAntiforgery(options =>
                    {
                        options.HeaderName = XSRF;
                        options.FormFieldName = RequestVerificationToken;
                        options.Cookie.HttpOnly = false;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                        options.Cookie.IsEssential = true;
                        options.SuppressXFrameOptionsHeader = false;
                    })
                .ConfigureAuthentication(this.Configuration)
                .AddAuthorization(options =>
                    {
                        options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser()
                            .Build();
                    })
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddSignalR();
            services.AddHealthChecks();
        }
    }
}