namespace LafAPI.Web.Infrastructure.Common
{
    using System.IO;

    public class ApplicationConstants
    {
        public const string Infrastructure = nameof(Infrastructure);
        public const string ConfigFiles = nameof(ConfigFiles);
        public const string Development = nameof(Development);
        public const string Staging = nameof(Staging);
        public const string Production = nameof(Production);
        public const string DefaultSettings = "appsettings.json";
        public const string StagingSettings = "appsettings.Staging.json";
        public const string DevelopmentSettings = "appsettings.Development.json";
        public const string JsonContentType = "application/json";
        public const string XSRF = "X-XSRF-TOKEN";
        public const string SessionKey = ".LafAPI.Session";
        public const string RequestVerificationToken = nameof(RequestVerificationToken);
        public const int JwtTokenExpirationDays = 15;

        public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
        public static readonly string ConfigPath = Path.Combine(CurrentDirectory, Infrastructure, ConfigFiles);
    }
}