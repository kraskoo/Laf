namespace LafAPI.Web.Infrastructure.Middlewares.Auth
{
    using System;
    using System.Threading.Tasks;

    using LafAPI.Web.Infrastructure.Common;

    using Microsoft.IdentityModel.Tokens;

    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(ApplicationConstants.JwtTokenExpirationDays);

        public Func<Task<string>> NonceGenerator { get; set; } = () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }
}