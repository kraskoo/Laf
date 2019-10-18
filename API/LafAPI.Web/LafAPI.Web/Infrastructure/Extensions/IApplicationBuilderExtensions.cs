namespace LafAPI.Web.Infrastructure.Extensions
{
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Middlewares.Auth;

    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using static LafAPI.Web.Infrastructure.Common.ApplicationConstants;

    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureWithAntiforgery(
            this IApplicationBuilder app,
            IAntiforgery antiforgery) =>
            app.Use(
                next => context =>
                    {
                        var path = context.Request.Path.Value;
                        if (path.Equals("/account/register", StringComparison.OrdinalIgnoreCase) ||
                            path.Equals("/account/login", StringComparison.OrdinalIgnoreCase))
                        {
                            var tokens = antiforgery.GetAndStoreTokens(context);
                            if (tokens.CookieToken != null)
                            {
                                context.Session.SetString(tokens.HeaderName, tokens.CookieToken);
                            }

                            context.Response.Cookies.Append(
                                RequestVerificationToken,
                                tokens.RequestToken,
                                new CookieOptions { HttpOnly = false });
                        }

                        return next(context);
                    });

        public static IApplicationBuilder ConfigureAuthentication(this IApplicationBuilder app)
        {
            async Task<GenericPrincipal> PrincipalResolver(HttpContext context) =>
                await context.RequestServices
                    .GetRequiredService<UserManager<User>>()
                    .PrincipalResolver(
                        context.Request.Form["email"],
                        context.Request.Form["password"]);
            var tokenProviderOptions = app.ApplicationServices.GetRequiredService<IOptions<TokenProviderOptions>>();
            app.UseJwtBearerTokens(tokenProviderOptions, PrincipalResolver);
            return app.UseAuthentication();
        }
    }
}