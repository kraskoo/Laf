namespace LafAPI.Web.Infrastructure.Extensions
{
    using System;

    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
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
                        if (path.Contains("/message/handshake", StringComparison.OrdinalIgnoreCase))
                        {
                            var tokens = antiforgery.GetAndStoreTokens(context);
                            context.Response.Cookies.Append(
                                XSRF,
                                tokens.RequestToken,
                                new CookieOptions { HttpOnly = false });
                        }

                        return next(context);
                    });
    }
}