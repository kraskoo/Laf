namespace LafAPI.Web.Infrastructure.Middlewares.Auth
{
    using System;
    using System.Net;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using LafAPI.Web.Infrastructure.Common;
    using LafAPI.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using Newtonsoft.Json;

    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TokenProviderOptions options;
        private readonly Func<HttpContext, Task<GenericPrincipal>> principalResolver;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            Func<HttpContext, Task<GenericPrincipal>> principalResolver)
        {
            this.next = next;
            this.options = options.Value;
            this.principalResolver = principalResolver;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(this.options.Path, StringComparison.Ordinal))
            {
                return this.next(context);
            }

            if (context.Request.Method.Equals("POST") && context.Request.HasFormContentType)
            {
                return this.GenerateToken(context);
            }

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync("Bad request");
        }

        private async Task GenerateToken(HttpContext context)
        {
            var principal = await this.principalResolver(context);
            var response = await context.GetLoginResponse(this.options, principal);
            if (response == null)
            {
                return;
            }

            context.Response.ContentType = ApplicationConstants.JsonContentType;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}