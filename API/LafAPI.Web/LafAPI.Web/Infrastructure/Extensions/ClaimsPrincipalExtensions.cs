namespace LafAPI.Web.Infrastructure.Extensions
{
    using System;
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            return claimsPrincipal.Identity.IsAuthenticated ?
                       claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value :
                       null;
        }
    }
}