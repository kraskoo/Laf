namespace LafAPI.Web.Infrastructure.Extensions
{
    using System;
    using System.Text;

    using LafAPI.Data;
    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Common;
    using LafAPI.Web.Infrastructure.Middlewares.Auth;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtTokenValidationKey = configuration["Environment"].PathToJwtTokenValidationKey();
            var secret = configuration[$"{jwtTokenValidationKey}:Secret"];
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            void ProviderOptions(TokenProviderOptions options)
            {
                options.Audience = configuration[$"{jwtTokenValidationKey}:Audience"];
                options.Issuer = configuration[$"{jwtTokenValidationKey}:Issuer"];
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
                            ValidIssuer = configuration[$"{jwtTokenValidationKey}:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration[$"{jwtTokenValidationKey}:Audience"],
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
            return services;
        }
    }
}