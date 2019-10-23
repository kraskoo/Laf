namespace LafAPI.Web.Infrastructure.Configurations
{
    using System;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    using LafAPI.Data;
    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Common;
    using LafAPI.Web.Infrastructure.Extensions;
    using LafAPI.Web.Infrastructure.Middlewares.Auth;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class IdentityConfiguration : BaseConfiguration
    {
        public IdentityConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            async Task<GenericPrincipal> PrincipalResolver(HttpContext context) =>
                await context.RequestServices
                    .GetRequiredService<UserManager<User>>()
                    .PrincipalResolver(
                        context.Request.Form["email"],
                        context.Request.Form["password"]);
            var tokenProviderOptions = app.ApplicationServices.GetRequiredService<IOptions<TokenProviderOptions>>();
            app.UseJwtBearerTokens(tokenProviderOptions, PrincipalResolver);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var jwtTokenValidationKey = this.Configuration["Environment"].PathToJwtTokenValidationKey();
            var secret = this.Configuration[$"{jwtTokenValidationKey}:Secret"];
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            void ProviderOptions(TokenProviderOptions options)
            {
                options.Audience = this.Configuration[$"{jwtTokenValidationKey}:Audience"];
                options.Issuer = this.Configuration[$"{jwtTokenValidationKey}:Issuer"];
                options.Path = "/auth/login";
                options.Expiration = TimeSpan.FromDays(ApplicationConstants.JwtTokenExpirationDays);
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
            }

            void JwtBearerOptions(JwtBearerOptions options) =>
                options.TokenValidationParameters =
                    new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey,
                            ValidateIssuer = true,
                            ValidIssuer = this.Configuration[$"{jwtTokenValidationKey}:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = this.Configuration[$"{jwtTokenValidationKey}:Audience"],
                            ValidateLifetime = true
                        };

            void IdentityOptions(IdentityOptions options)
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }

            services.Configure<TokenProviderOptions>(ProviderOptions);
            services.AddAuthentication().AddJwtBearer(JwtBearerOptions);
            services.AddIdentity<User, Role>(IdentityOptions)
                .AddEntityFrameworkStores<LafContext>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();
        }
    }
}