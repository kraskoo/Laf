namespace LafAPI.Web.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Middlewares.Auth;
    using LafAPI.Web.Infrastructure.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class HttpContextExtensions
    {
        public static async Task<LoginResponse> GetLoginResponse(
            this HttpContext context,
            TokenProviderOptions options,
            GenericPrincipal principal)
        {
            if (principal == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Invalid email or password.");
                return default;
            }

            var now = DateTime.UtcNow;
            var unixTimeSeconds = (long)Math.Round(
                (now.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
            var existingClaims = principal.Claims.ToList();
            var systemClaims =
                new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, principal.Identity.Name),
                        new Claim(JwtRegisteredClaimNames.Jti, await options.NonceGenerator()),
                        new Claim(
                            JwtRegisteredClaimNames.Iat,
                            unixTimeSeconds.ToString(),
                            ClaimValueTypes.Integer64)
                    };
            foreach (var systemClaim in systemClaims)
            {
                var existingClaimIndex = GetClaimIndex(existingClaims, systemClaim.Type);
                if (existingClaimIndex < 0)
                {
                    existingClaims.Add(systemClaim);
                }
                else
                {
                    existingClaims[existingClaimIndex] = systemClaim;
                }
            }

            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: existingClaims,
                notBefore: now,
                expires: now.Add(options.Expiration),
                signingCredentials: options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new LoginResponse
                    {
                        Token = encodedJwt,
                        ExpiresIn = (int)options.Expiration.TotalMilliseconds,
                        Roles = existingClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
                    };
        }

        public static async Task<GenericPrincipal> PrincipalResolver(
            this HttpContext context,
            string email,
            string password) =>
            await context.RequestServices
                .GetRequiredService<UserManager<User>>()
                .PrincipalResolver(email, password);

        private static int GetClaimIndex(IList<Claim> claims, string type)
        {
            for (var i = 0; i < claims.Count; i++)
            {
                if (claims[i].Type == type)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}