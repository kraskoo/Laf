namespace LafAPI.Web.Infrastructure.Common
{
    public static class Provider
    {
        public static string PathToKeys(this string environment) =>
            environment == ApplicationConstants.Development ? "Logging:" : string.Empty;

        public static string PathToJwtTokenValidationKey(this string environment) =>
            environment == ApplicationConstants.Development ? "Logging:JwtTokenValidation" : "JwtTokenValidation";
    }
}